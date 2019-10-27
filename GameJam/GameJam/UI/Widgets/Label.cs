using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace GameJam.UI.Widgets
{
    public class Label : Widget
    {
        private readonly BitmapFont _font;
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

                Size2 dimensions = _font.MeasureString(_content);

                AspectRatio = (float)dimensions.Width / dimensions.Height;
                MaintainAspectRatio = true;

                _bounds = dimensions;

                ComputeProperties();
            }
        }

        public Label(BitmapFont font,
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Hidden)
            {
                Vector2 scale = new Vector2(BottomRight.X - TopLeft.X, BottomRight.Y - TopLeft.Y) / _bounds;
                spriteBatch.DrawString(_font,
                    _content,
                    TopLeft,
                    TintColor,
                    0,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0);
            }
        }

        public override bool Handle(IEvent evt)
        {
            return false;
        }
    }
}
