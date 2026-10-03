// Converts the Xbox 360 content of Squadron Scramble into XNB files MonoGame can load on PC.
//
//   * Extracts the files from the Xbox Live Indie Game (STFS) package.
//   * Decompresses LZX (XMemCompress) XNB files.
//   * Byte-swaps texture data (Xbox 360 is big-endian).
//   * Decodes XMA2 sound effects with ffmpeg and re-encodes them as MS-ADPCM
//     (or 16-bit PCM with --pcm), both of which MonoGame can play.
//   * Writes uncompressed Windows ('w') XNB files.
//
// Usage: XboxContentConverter <package-file> <output-content-dir> [--extract <dir>] [--pcm]
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace XboxContentConverter
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: XboxContentConverter <package-file> <output-content-dir> [--extract <dir>] [--pcm]");
                return 1;
            }

            string packagePath = args[0];
            string outDir = args[1];
            string extractDir = null;
            for (int i = 2; i < args.Length; i++)
            {
                if (args[i] == "--extract" && i + 1 < args.Length)
                    extractDir = args[++i];
                else if (args[i] == "--pcm")
                    XnbConverter.UsePcm = true;
            }

            var package = new StfsPackage(packagePath);
            Console.WriteLine($"Package: {package.DisplayName}");
            Directory.CreateDirectory(outDir);

            int converted = 0;
            foreach (var entry in package.Entries.Where(e => !e.IsDirectory))
            {
                byte[] bytes = package.ReadFile(entry);
                if (extractDir != null)
                {
                    string dest = Path.Combine(extractDir, entry.Path);
                    Directory.CreateDirectory(Path.GetDirectoryName(dest));
                    File.WriteAllBytes(dest, bytes);
                }

                int contentIndex = entry.Path.IndexOf("/Content/", StringComparison.OrdinalIgnoreCase);
                if (contentIndex < 0 || !entry.Path.EndsWith(".xnb", StringComparison.OrdinalIgnoreCase))
                    continue;

                string relative = entry.Path.Substring(contentIndex + "/Content/".Length);
                string outPath = Path.Combine(outDir, relative);
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                try
                {
                    File.WriteAllBytes(outPath, XnbConverter.Convert(bytes, relative));
                    converted++;
                    Console.WriteLine($"  {relative}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"  FAILED {relative}: {ex.Message}");
                    return 2;
                }
            }

            Console.WriteLine($"Converted {converted} XNB files to {outDir}");
            return 0;
        }
    }

    internal static class XnbConverter
    {
        /// <summary>Write lossless 16-bit PCM instead of MS-ADPCM (about 4x larger).</summary>
        public static bool UsePcm;

        private const string Texture2DReader = "Microsoft.Xna.Framework.Content.Texture2DReader";
        private const string SpriteFontReader = "Microsoft.Xna.Framework.Content.SpriteFontReader";
        private const string SoundEffectReader = "Microsoft.Xna.Framework.Content.SoundEffectReader";

        public static byte[] Convert(byte[] xnb, string name)
        {
            if (xnb.Length < 10 || xnb[0] != 'X' || xnb[1] != 'N' || xnb[2] != 'B')
                throw new InvalidDataException("Not an XNB file.");
            byte flags = xnb[5];
            byte[] payload = Decompress(xnb);

            var reader = new BinaryReader(new MemoryStream(payload));
            int readerCount = Read7BitInt(reader);
            var readers = new List<string>();
            for (int i = 0; i < readerCount; i++)
            {
                readers.Add(reader.ReadString().Split(',')[0]);
                reader.ReadInt32();
            }
            int sharedCount = Read7BitInt(reader);
            if (sharedCount != 0)
                throw new NotSupportedException("Shared resources are not supported.");
            int typeId = Read7BitInt(reader);
            string primary = readers[typeId - 1];
            int objectStart = (int)reader.BaseStream.Position;

            byte[] newPayload;
            switch (primary)
            {
                case Texture2DReader:
                    SwapTexture(payload, objectStart);
                    newPayload = payload;
                    break;
                case SpriteFontReader:
                    reader.BaseStream.Position = objectStart;
                    Read7BitInt(reader); // inline Texture2D type id
                    SwapTexture(payload, (int)reader.BaseStream.Position);
                    newPayload = payload;
                    break;
                case SoundEffectReader:
                    newPayload = ConvertSoundEffect(payload, objectStart, name);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported content type {primary}.");
            }

            var output = new MemoryStream();
            var w = new BinaryWriter(output);
            w.Write(Encoding.ASCII.GetBytes("XNBw"));
            w.Write((byte)5);
            w.Write((byte)(flags & 0x01)); // keep HiDef bit, drop compression
            w.Write(newPayload.Length + 10);
            w.Write(newPayload);
            return output.ToArray();
        }

        private static byte[] Decompress(byte[] xnb)
        {
            byte flags = xnb[5];
            int fileSize = BitConverter.ToInt32(xnb, 6);
            if ((flags & 0x80) == 0)
                return xnb.AsSpan(10, fileSize - 10).ToArray();

            int decompressedSize = BitConverter.ToInt32(xnb, 10);
            var input = new MemoryStream(xnb, 14, xnb.Length - 14);
            // MonoGame's LZX decoder is internal; it is the same XMemCompress format XNA uses.
            var type = typeof(Microsoft.Xna.Framework.Game).Assembly.GetType("MonoGame.Framework.Utilities.LzxDecoderStream", true);
            using var lzx = (Stream)Activator.CreateInstance(type, input, decompressedSize, fileSize - 14);
            var result = new byte[decompressedSize];
            int read = 0;
            while (read < decompressedSize)
            {
                int n = lzx.Read(result, read, decompressedSize - read);
                if (n <= 0)
                    throw new InvalidDataException("Truncated LZX stream.");
                read += n;
            }
            return result;
        }

        private static void SwapTexture(byte[] payload, int offset)
        {
            int format = BitConverter.ToInt32(payload, offset);
            int levels = BitConverter.ToInt32(payload, offset + 12);
            int pos = offset + 16;
            for (int level = 0; level < levels; level++)
            {
                int size = BitConverter.ToInt32(payload, pos);
                pos += 4;
                switch (format)
                {
                    case 0: // Color: Xbox stores ARGB as big-endian words, PC wants RGBA bytes
                        for (int i = pos; i + 3 < pos + size; i += 4)
                        {
                            (payload[i], payload[i + 3]) = (payload[i + 3], payload[i]);
                            (payload[i + 1], payload[i + 2]) = (payload[i + 2], payload[i + 1]);
                        }
                        break;
                    case 1: // Bgr565
                    case 2: // Bgra5551
                    case 3: // Bgra4444
                    case 4: // Dxt1
                    case 5: // Dxt3
                    case 6: // Dxt5
                        for (int i = pos; i + 1 < pos + size; i += 2)
                            (payload[i], payload[i + 1]) = (payload[i + 1], payload[i]);
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported surface format {format}.");
                }
                pos += size;
            }
        }

        private static byte[] ConvertSoundEffect(byte[] payload, int offset, string name)
        {
            var r = new BinaryReader(new MemoryStream(payload));
            r.BaseStream.Position = offset;
            int formatSize = r.ReadInt32();
            byte[] format = r.ReadBytes(formatSize);
            int dataSize = r.ReadInt32();
            byte[] data = r.ReadBytes(dataSize);
            r.ReadInt32(); // loop start
            r.ReadInt32(); // loop length
            r.ReadInt32(); // duration

            ushort formatTag = (ushort)((format[0] << 8) | format[1]);
            if (BitConverter.ToUInt16(format, 0) == 1)
                return payload; // already little-endian PCM
            if (formatTag != 0x0166)
                throw new NotSupportedException($"Unsupported audio format 0x{formatTag:X4}.");

            // XMA2WAVEFORMATEX is stored big-endian; rewrite it little-endian for a RIFF file ffmpeg understands.
            byte[] xmaFormat = SwapXma2Format(format);
            Wave wave = DecodeWithFfmpeg(xmaFormat, data, name);

            var ms = new MemoryStream();
            var w = new BinaryWriter(ms);
            w.Write(payload, 0, offset); // type readers + type id
            w.Write(wave.Format.Length);
            w.Write(wave.Format);
            w.Write(wave.Data.Length);
            w.Write(wave.Data);
            w.Write(0);                   // loop start
            w.Write(wave.Samples);        // loop length
            w.Write((int)(wave.Samples * 1000L / wave.SampleRate));
            return ms.ToArray();
        }

        private sealed class Wave
        {
            public byte[] Format;
            public byte[] Data;
            public int Samples;
            public int SampleRate;
        }

        private static byte[] SwapXma2Format(byte[] be)
        {
            // WAVEFORMATEX (2,2,4,4,2,2,2) + XMA2 extension (2,4,4,4,4,4,4,4,1,1,2)
            int[] sizes = { 2, 2, 4, 4, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 4, 1, 1, 2 };
            var le = (byte[])be.Clone();
            int pos = 0;
            foreach (int size in sizes)
            {
                if (pos + size > le.Length)
                    break;
                Array.Reverse(le, pos, size);
                pos += size;
            }
            return le;
        }

        private static Wave DecodeWithFfmpeg(byte[] format, byte[] data, string name)
        {
            string tmp = Path.Combine(Path.GetTempPath(), "xcc_" + Guid.NewGuid().ToString("N"));
            string input = tmp + ".in.wav";
            string output = tmp + ".out.wav";
            try
            {
                using (var fs = File.Create(input))
                using (var w = new BinaryWriter(fs))
                {
                    w.Write(Encoding.ASCII.GetBytes("RIFF"));
                    w.Write(4 + 8 + format.Length + 8 + data.Length);
                    w.Write(Encoding.ASCII.GetBytes("WAVE"));
                    w.Write(Encoding.ASCII.GetBytes("fmt "));
                    w.Write(format.Length);
                    w.Write(format);
                    w.Write(Encoding.ASCII.GetBytes("data"));
                    w.Write(data.Length);
                    w.Write(data);
                }

                var psi = new ProcessStartInfo("ffmpeg")
                {
                    RedirectStandardError = true,
                    UseShellExecute = false
                };
                string codec = UsePcm ? "pcm_s16le" : "adpcm_ms";
                foreach (string a in new[] { "-v", "error", "-y", "-i", input, "-acodec", codec, "-f", "wav", "-bitexact", output })
                    psi.ArgumentList.Add(a);
                Process p;
                try
                {
                    p = Process.Start(psi);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("ffmpeg is required to decode XMA2 audio but could not be started: " + ex.Message);
                }
                string err = p.StandardError.ReadToEnd();
                p.WaitForExit();
                if (p.ExitCode != 0)
                    throw new InvalidOperationException($"ffmpeg failed for {name}: {err}");
                return ReadWave(File.ReadAllBytes(output));
            }
            finally
            {
                File.Delete(input);
                File.Delete(output);
            }
        }

        private static Wave ReadWave(byte[] riff)
        {
            var wave = new Wave { Samples = -1 };
            int pos = 12;
            while (pos + 8 <= riff.Length)
            {
                string id = Encoding.ASCII.GetString(riff, pos, 4);
                int size = BitConverter.ToInt32(riff, pos + 4);
                int body = pos + 8;
                switch (id)
                {
                    case "fmt ":
                        wave.Format = riff.AsSpan(body, size).ToArray();
                        break;
                    case "data":
                        wave.Data = riff.AsSpan(body, Math.Min(size, riff.Length - body)).ToArray();
                        break;
                    case "fact":
                        wave.Samples = BitConverter.ToInt32(riff, body);
                        break;
                }
                pos = body + size + (size & 1);
            }
            if (wave.Format == null || wave.Data == null)
                throw new InvalidDataException("ffmpeg produced an invalid WAV file.");
            if (BitConverter.ToUInt16(wave.Format, 0) == 0xFFFE && wave.Format.Length >= 40)
            {
                // WAVE_FORMAT_EXTENSIBLE is not understood by MonoGame: turn it back into a plain
                // WAVEFORMATEX using the sub-format tag, keeping any codec-specific data that follows.
                int cbSize = BitConverter.ToUInt16(wave.Format, 16) - 22;
                var plain = new byte[18 + Math.Max(0, cbSize)];
                Buffer.BlockCopy(wave.Format, 0, plain, 0, 18);
                Buffer.BlockCopy(wave.Format, 24, plain, 0, 2); // sub-format GUID starts with the format tag
                BitConverter.GetBytes((ushort)Math.Max(0, cbSize)).CopyTo(plain, 16);
                if (cbSize > 0)
                    Buffer.BlockCopy(wave.Format, 40, plain, 18, cbSize);
                wave.Format = plain;
            }
            wave.SampleRate = BitConverter.ToInt32(wave.Format, 4);
            if (wave.Samples < 0)
                wave.Samples = wave.Data.Length / BitConverter.ToUInt16(wave.Format, 12);
            return wave;
        }

        private static int Read7BitInt(BinaryReader r)
        {
            int result = 0, shift = 0;
            byte b;
            do
            {
                b = r.ReadByte();
                result |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return result;
        }
    }
}
