using Microsoft.Xna.Framework.Graphics;

namespace FontExtension
{
    public sealed class GlyphRenderInfo
    {
        public char Character { get; }
        public Texture2D Texture { get; }
        public Metrics Metrics { get; }

        public GlyphRenderInfo(char character, Texture2D texture, Metrics metrics)
        {
            Character = character;
            Texture = texture;
            Metrics = metrics;
        }
    }
}
