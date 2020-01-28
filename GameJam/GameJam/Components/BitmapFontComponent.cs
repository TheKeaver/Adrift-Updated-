using Audrey;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace GameJam.Components
{
    /// <summary>
    /// A component for renderable text.
    /// </summary>
    public class BitmapFontComponent : IComponent
    {
        public BitmapFontComponent()
        {
        }

        public BitmapFontComponent(BitmapFont font, string content)
        {
            Font = font;
            Content = content;
        }

        public BitmapFont Font;
        public string Content;
        public Color Color = Color.White;

        public byte RenderGroup = GameJam.Constants.Render.GROUP_ONE;

        public bool Hidden;

        public float Depth = 0;
    }
}
