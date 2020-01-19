using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace FontExtension
{
    public sealed class GlyphRenderInfo
    {
        public char Character { get; }
        public TextureRegion2D TextureRegion { get; }
        public Metrics Metrics { get; }
        public GlyphRenderInfo(char character, TextureRegion2D textureRegion, Metrics metrics)
        {
            Character = character;
            TextureRegion = textureRegion;
            Metrics = metrics;
        }
    }
}
