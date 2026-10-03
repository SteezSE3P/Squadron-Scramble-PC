// The Xbox build references VideoPlayer but never loads or plays a video, and MonoGame
// DesktopGL has no video support, so a stub that always reports Stopped is enough.
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Media
{
    public sealed class Video
    {
    }

    public sealed class VideoPlayer
    {
        public MediaState State => MediaState.Stopped;

        public bool IsLooped { get; set; }

        public void Play(Video video)
        {
        }

        public void Stop()
        {
        }

        public Texture2D GetTexture() => null;
    }
}
