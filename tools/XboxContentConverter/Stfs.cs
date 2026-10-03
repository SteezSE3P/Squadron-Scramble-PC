using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XboxContentConverter
{
    /// <summary>
    /// Minimal reader for Xbox 360 STFS packages (LIVE/PIRS/CON), enough to extract
    /// the files of an Xbox Live Indie Game package.
    /// </summary>
    public sealed class StfsPackage
    {
        private const int BlockSize = 0x1000;
        private readonly byte[] data;
        private readonly int baseOffset;
        private readonly int shift;
        private readonly int level0Step;

        public sealed class Entry
        {
            public string Path;
            public bool IsDirectory;
            public int Size;
            internal int StartBlock;
            internal int BlockCount;
            internal bool Consecutive;
        }

        public List<Entry> Entries { get; } = new List<Entry>();

        public string DisplayName { get; }

        public StfsPackage(string path)
        {
            data = File.ReadAllBytes(path);
            string magic = Encoding.ASCII.GetString(data, 0, 4);
            if (magic != "LIVE" && magic != "PIRS" && magic != "CON ")
                throw new InvalidDataException($"{path} is not an Xbox 360 STFS package.");

            DisplayName = Encoding.BigEndianUnicode.GetString(data, 0x411, 0x80).TrimEnd('\0');

            int headerSize = ReadInt32BE(0x340);
            baseOffset = (headerSize + 0xFFF) & ~0xFFF;

            // Volume descriptor at 0x379
            byte blockSeparation = data[0x379 + 2];
            int fileTableBlockCount = data[0x379 + 3] | (data[0x379 + 4] << 8);
            int fileTableStart = data[0x379 + 5] | (data[0x379 + 6] << 8) | (data[0x379 + 7] << 16);

            // "Female" packages (read-only, e.g. LIVE content) have one hash table per level.
            bool female = (blockSeparation & 1) == 1;
            shift = female ? 0 : 1;
            level0Step = female ? 0xAB : 0xAC;

            var raw = new List<(string name, bool dir, bool consec, int blocks, int start, short parent, int size)>();
            int block = fileTableStart;
            for (int i = 0; i < fileTableBlockCount; i++)
            {
                int off = BlockToOffset(block);
                for (int j = 0; j < BlockSize / 0x40; j++)
                {
                    int e = off + j * 0x40;
                    byte flags = data[e + 0x28];
                    int nameLen = flags & 0x3F;
                    if (nameLen == 0)
                        continue;
                    string name = Encoding.ASCII.GetString(data, e, nameLen);
                    int blocks = data[e + 0x29] | (data[e + 0x2A] << 8) | (data[e + 0x2B] << 16);
                    int start = data[e + 0x2F] | (data[e + 0x30] << 8) | (data[e + 0x31] << 16);
                    short parent = (short)((data[e + 0x32] << 8) | data[e + 0x33]);
                    int size = ReadInt32BE(e + 0x34);
                    raw.Add((name, (flags & 0x80) != 0, (flags & 0x40) != 0, blocks, start, parent, size));
                }
                block = NextBlock(block);
            }

            string PathOf(int index)
            {
                var r = raw[index];
                return r.parent < 0 ? r.name : PathOf(r.parent) + "/" + r.name;
            }

            for (int i = 0; i < raw.Count; i++)
            {
                var r = raw[i];
                Entries.Add(new Entry
                {
                    Path = PathOf(i),
                    IsDirectory = r.dir,
                    Size = r.size,
                    StartBlock = r.start,
                    BlockCount = r.blocks,
                    Consecutive = r.consec
                });
            }
        }

        public byte[] ReadFile(Entry entry)
        {
            var result = new byte[entry.Size];
            int block = entry.StartBlock;
            int written = 0;
            for (int i = 0; i < entry.BlockCount && written < entry.Size; i++)
            {
                int count = Math.Min(BlockSize, entry.Size - written);
                Buffer.BlockCopy(data, BlockToOffset(block), result, written, count);
                written += count;
                block = entry.Consecutive ? block + 1 : NextBlock(block);
            }
            return result;
        }

        private int BlockToOffset(int block)
        {
            int backing = (((block + 0xAA) / 0xAA) << shift) + block;
            if (block >= 0xAA)
            {
                backing += ((block + 0x70E4) / 0x70E4) << shift;
                if (block >= 0x70E4)
                    backing += 1 << shift;
            }
            return baseOffset + backing * BlockSize;
        }

        private int NextBlock(int block)
        {
            int hashBlock = 0;
            if (block >= 0xAA)
            {
                hashBlock = (block / 0xAA) * level0Step + (((block / 0x70E4) + 1) << shift);
                if (block / 0x70E4 != 0)
                    hashBlock += 1 << shift;
            }
            int entry = baseOffset + hashBlock * BlockSize + (block % 0xAA) * 0x18;
            return (data[entry + 0x15] << 16) | (data[entry + 0x16] << 8) | data[entry + 0x17];
        }

        private int ReadInt32BE(int offset)
        {
            return (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
        }
    }
}
