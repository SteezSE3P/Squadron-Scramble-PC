// PC replacement for the Xbox 360-only Microsoft.Xna.Framework.Storage API.
// Containers map to folders under the user's application-data directory.
//
// During an online game every PC uses an in-memory copy of the host's save files instead,
// so all PCs load the same pilot names and settings (the host also writes changes to disk).
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Microsoft.Xna.Framework.Storage
{
    public sealed class StorageDevice
    {
        public static string RootPath
        {
            get
            {
                string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (string.IsNullOrEmpty(baseDir))
                    baseDir = AppContext.BaseDirectory;
                return Path.Combine(baseDir, "SquadronScramble");
            }
        }

        public bool IsConnected => true;

        /// <summary>When set, containers read and write these files (keyed "container/file") instead of the disk.</summary>
        public static Dictionary<string, byte[]> VirtualFiles;

        /// <summary>With <see cref="VirtualFiles"/>: also write changes to disk.</summary>
        public static bool VirtualWriteThrough;

        public static void UseVirtualFiles(Dictionary<string, byte[]> files, bool writeThrough)
        {
            VirtualFiles = files != null ? new Dictionary<string, byte[]>(files, StringComparer.OrdinalIgnoreCase) : null;
            VirtualWriteThrough = writeThrough;
        }

        public static IAsyncResult BeginShowSelector(AsyncCallback callback, object state)
        {
            return CompletedAsyncResult.Complete(new StorageDevice(), callback, state);
        }

        public static IAsyncResult BeginShowSelector(PlayerIndex player, AsyncCallback callback, object state)
        {
            return BeginShowSelector(callback, state);
        }

        public static StorageDevice EndShowSelector(IAsyncResult result)
        {
            return (StorageDevice)((CompletedAsyncResult)result).Result;
        }

        public IAsyncResult BeginOpenContainer(string displayName, AsyncCallback callback, object state)
        {
            return CompletedAsyncResult.Complete(new StorageContainer(displayName), callback, state);
        }

        public StorageContainer EndOpenContainer(IAsyncResult result)
        {
            return (StorageContainer)((CompletedAsyncResult)result).Result;
        }
    }

    public sealed class StorageContainer : IDisposable
    {
        private readonly string path;
        private readonly string folder;

        internal StorageContainer(string displayName)
        {
            DisplayName = displayName;
            foreach (char c in Path.GetInvalidFileNameChars())
                displayName = displayName.Replace(c, '_');
            folder = displayName;
            path = Path.Combine(StorageDevice.RootPath, displayName);
            if (StorageDevice.VirtualFiles == null || StorageDevice.VirtualWriteThrough)
                Directory.CreateDirectory(path);
        }

        public string DisplayName { get; }

        public bool IsDisposed { get; private set; }

        private static Dictionary<string, byte[]> Virtual => StorageDevice.VirtualFiles;

        private string Key(string file) => folder + "/" + file.Replace('\\', '/');

        public bool FileExists(string file) => Virtual != null ? Virtual.ContainsKey(Key(file)) : File.Exists(Path.Combine(path, file));

        public void DeleteFile(string file)
        {
            if (Virtual != null)
            {
                Virtual.Remove(Key(file));
                if (!StorageDevice.VirtualWriteThrough)
                    return;
                try { File.Delete(Path.Combine(path, file)); } catch (Exception) { }
                return;
            }
            File.Delete(Path.Combine(path, file));
        }

        public Stream CreateFile(string file)
        {
            if (Virtual != null)
                return new VirtualFileStream(Key(file), StorageDevice.VirtualWriteThrough ? Path.Combine(path, file) : null);
            return File.Create(Path.Combine(path, file));
        }

        public Stream OpenFile(string file, FileMode mode) => OpenFile(file, mode, mode == FileMode.Open ? FileAccess.Read : FileAccess.ReadWrite);

        public Stream OpenFile(string file, FileMode mode, FileAccess access)
        {
            if (Virtual != null)
            {
                if (Virtual.TryGetValue(Key(file), out byte[] data) && mode != FileMode.Create && mode != FileMode.Truncate)
                {
                    if (access == FileAccess.Read)
                        return new MemoryStream(data, false);
                    var s = new VirtualFileStream(Key(file), StorageDevice.VirtualWriteThrough ? Path.Combine(path, file) : null);
                    s.Write(data, 0, data.Length);
                    s.Position = 0;
                    return s;
                }
                if (mode == FileMode.Open || mode == FileMode.Truncate)
                    throw new FileNotFoundException(file);
                return CreateFile(file);
            }
            return File.Open(Path.Combine(path, file), mode, access);
        }

        public void Dispose() => IsDisposed = true;

        /// <summary>A memory stream that stores its contents in <see cref="StorageDevice.VirtualFiles"/> when closed.</summary>
        private sealed class VirtualFileStream : MemoryStream
        {
            private readonly string key;
            private readonly string diskPath;
            private bool committed;

            public VirtualFileStream(string key, string diskPath)
            {
                this.key = key;
                this.diskPath = diskPath;
            }

            protected override void Dispose(bool disposing)
            {
                if (!committed && Virtual != null)
                {
                    committed = true;
                    byte[] data = ToArray();
                    Virtual[key] = data;
                    if (diskPath != null)
                    {
                        try { File.WriteAllBytes(diskPath, data); } catch (Exception) { }
                    }
                }
                base.Dispose(disposing);
            }
        }
    }

    /// <summary>An IAsyncResult that has already completed; used to emulate the Xbox async storage/guide calls.</summary>
    internal sealed class CompletedAsyncResult : IAsyncResult
    {
        private ManualResetEvent waitHandle;

        public object Result { get; set; }

        public object AsyncState { get; set; }

        public bool IsCompleted { get; set; } = true;

        public bool CompletedSynchronously => true;

        public WaitHandle AsyncWaitHandle => waitHandle ??= new ManualResetEvent(IsCompleted);

        internal void SetCompleted()
        {
            IsCompleted = true;
            waitHandle?.Set();
        }

        public static CompletedAsyncResult Complete(object result, AsyncCallback callback, object state)
        {
            var r = new CompletedAsyncResult { Result = result, AsyncState = state };
            callback?.Invoke(r);
            return r;
        }
    }
}
