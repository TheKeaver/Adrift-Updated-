using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.UI.Widgets
{
    /// <summary>
    /// A UI widget for an image.
    /// </summary>
    public class Image : Widget
    {
        Texture2D _texture;
        Vector2 _bounds;

        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
                _bounds = new Vector2(_texture.Width, _texture.Height);
            }
        }

        public Image(Texture2D texture,
                      Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentAspect,
                      float pOffsetAspect,
                      float aspectRatio,
                      AspectRatioType aspectRatioType)
            : base(origin, percentX, pOffsetX, percentY, pOffsetY,
                   percentAspect, pOffsetAspect, aspectRatio, aspectRatioType)
        {
            _texture = texture;
            _bounds = new Vector2(_texture.Width, _texture.Height);
        }

        public Image(Texture2D texture,
                      Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentWidth,
                      float pOffsetWidth,
                      float percentHeight,
                      float pOffsetHeight)
            : base(origin, percentX, pOffsetX, percentY, pOffsetY,
                 percentWidth, pOffsetWidth, percentHeight, pOffsetHeight)
        {
            _texture = texture;
            _bounds = new Vector2(_texture.Width, _texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                Vector2 scale = (BottomRight - TopLeft) / _bounds;
                spriteBatch.Draw(_texture,
                                 TopLeft,
                                 null,
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
