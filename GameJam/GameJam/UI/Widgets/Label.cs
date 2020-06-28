using Events;
using FontExtension;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.UI.Widgets
{
    public class Label : Widget
    {
        private readonly FieldFont _font;
        private Vector2 _bounds;
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;

                Vector2 dimensions = _font.MeasureString(_content);

                AspectRatio = dimensions.X / dimensions.Y;
                MaintainAspectRatio = true;

                _bounds = dimensions;

                ComputeProperties();
            }
        }

        public Label(FieldFont font,
            string content,
            HorizontalAlignment hAlign,
            AbstractValue horizontal,
            VerticalAlignment vAlign,
            AbstractValue vertical,
            AbstractValue width,
            AbstractValue height) : base(hAlign, horizontal, vAlign, vertical, width, height)
        {
            _font = font;
            Content = content;
        }

        public override void Render(SpriteBatch spriteBatch, FieldFontRenderer fieldFontRenderer)
        {
            if(!Hidden)
            {
                // Labels maintain aspect-ratio, so it shouldn't matter which
                // axis we get the scale from.
                float scale = (BottomRight.X - TopLeft.X) / _bounds.X;

                Vector2 position = (BottomRight - TopLeft) / 2 + TopLeft;
                position.Y = Root.Height - position.Y;
                fieldFontRenderer.Draw(_font,
                    Content,
                    position,
                    0,
                    TintColor,
                    scale,
                    true);
            }
        }

        public override bool Handle(IEvent evt)
        {
            return false;
        }
    }
}
