using System.Collections.Generic;
using Audrey;
using FontExtension;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class FieldFontComponent : IComponent
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

        public byte RenderGroup = Constants.Render.GROUP_ONE;

        public bool Hidden;

        public bool OptimizeForSmallText = false;
        public bool EnableKerning = true;

        public float Depth = 0;
    }
}
