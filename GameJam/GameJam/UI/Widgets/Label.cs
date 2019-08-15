using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace GameJam.UI.Widgets
{
    /// <summary>
    /// A UI widget for a text label.
    /// </summary>
    public class Label : Widget
    {
        readonly BitmapFont _font;
        Vector2 _bounds;
        string _content;
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
                AspectRatio = dimensions.Width / dimensions.Height;

                _bounds = dimensions;

                ComputeProperties();
            }
        }

        public Label(BitmapFont font,
                      Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentAspect,
                      float pOffsetAspect,
                      AspectRatioType aspectRatioType)
            : base(origin, percentX, pOffsetX, percentY, pOffsetY,
                   percentAspect, pOffsetAspect, 0, aspectRatioType)
        {
            _font = font;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                Vector2 scale = new Vector2(Width, Height) / _bounds;
                spriteBatch.DrawString(_font,
                                       _content,
                                       TopLeft,
                                       Color.White,
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
