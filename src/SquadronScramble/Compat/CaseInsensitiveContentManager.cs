// The Xbox 360 (and Windows) file systems are case-insensitive and the game code does not
// always match the case of its asset names (e.g. "MySpriteFont3" vs "mySpriteFont3.xnb").
// This ContentManager resolves asset names case-insensitively so it also works on Linux/macOS.
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace SquadronScramble
{
    public class CaseInsensitiveContentManager : ContentManager
    {
        private Dictionary<string, string> files;

        public CaseInsensitiveContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }

        protected override Stream OpenStream(string assetName)
        {
            return base.OpenStream(Resolve(assetName));
        }

        private string Resolve(string assetName)
        {
            if (files == null)
            {
                files = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                string root = Path.Combine(AppContext.BaseDirectory, RootDirectory);
                if (Directory.Exists(root))
                {
                    foreach (string path in Directory.EnumerateFiles(root, "*.xnb", SearchOption.AllDirectories))
                    {
                        string relative = Path.GetRelativePath(root, path);
                        string name = relative.Substring(0, relative.Length - 4).Replace('\\', '/');
                        files[name] = name;
                    }
                }
            }
            string key = assetName.Replace('\\', '/');
            return files.TryGetValue(key, out string actual) ? actual : assetName;
        }
    }
}
