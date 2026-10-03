// PC replacement for the Xbox 360-only Microsoft.Xna.Framework.Storage API.
// Containers map to folders under the user's application-data directory.
using System;
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

        internal StorageContainer(string displayName)
        {
            DisplayName = displayName;
            foreach (char c in Path.GetInvalidFileNameChars())
                displayName = displayName.Replace(c, '_');
            path = Path.Combine(StorageDevice.RootPath, displayName);
            Directory.CreateDirectory(path);
        }

        public string DisplayName { get; }

        public bool IsDisposed { get; private set; }

        public bool FileExists(string file) => File.Exists(Path.Combine(path, file));

        public void DeleteFile(string file) => File.Delete(Path.Combine(path, file));

        public Stream CreateFile(string file) => File.Create(Path.Combine(path, file));

        public Stream OpenFile(string file, FileMode mode) => File.Open(Path.Combine(path, file), mode);

        public Stream OpenFile(string file, FileMode mode, FileAccess access) => File.Open(Path.Combine(path, file), mode, access);

        public void Dispose() => IsDisposed = true;
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
