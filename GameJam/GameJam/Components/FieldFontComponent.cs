using Audrey;
using FontExtension;
using GameJam.Common;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class FieldFontComponent : IComponent, IRenderComponent
    {
        public FieldFontComponent()
        {
        }

        public FieldFontComponent(FieldFont font, string content)
        {
            Font = font;
            Content = content;
        }

        public FieldFont Font;
        public string Content;
        public Color Color = Color.White;
        public float Alpha = 1;

        public byte RenderGroup = Constants.Render.GROUP_ONE;

        public bool Hidden;

        public bool OptimizeForSmallText = false;
        public bool EnableKerning = true;

        public float Depth = 0;

        public BoundingRect GetAABB(float scale)
        {
            Vector2 size = Font.MeasureString(Content, EnableKerning);
            return new BoundingRect(-size.X / 2 * scale, -size.Y / 2 * scale,
                size.X * scale, size.Y * scale);
        }

        public bool IsHidden()
        {
            return Hidden;
        }

        public byte GetRenderGroup()
        {
            return RenderGroup;
        }

        public float GetDepth()
        {
            return Depth;
        }
    }
}
