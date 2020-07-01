using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace GameJam.Components
{
    /// <summary>
    /// A component for renderable text.
    /// </summary>
    public class BitmapFontComponent : IComponent, IRenderComponent
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

        public BoundingRect GetAABB(float scale)
        {
            Size2 size = Font.MeasureString(Content);
            return new BoundingRect(-size.Width / 2 * scale, -size.Height / 2 * scale,
                size.Width * scale, size.Height * scale);
        }

        public bool IsHidden()
        {
            return Hidden;
        }

        public byte GetRenderGroup()
        {
            return RenderGroup;
        }
    }
}
