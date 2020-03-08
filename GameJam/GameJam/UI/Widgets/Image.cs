using Events;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.UI.Widgets
{
    public class Image : Widget
    {
        private TextureRegion2D _texture;
        private Vector2 _bounds;
        public TextureRegion2D Texture {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;

                _bounds = new Vector2(_texture.Width, _texture.Height);

                ComputeProperties();
            }
        }

        public Image(TextureRegion2D texture,

            HorizontalAlignment hAlign,
            AbstractValue horizontal,

            VerticalAlignment vAlign,
            AbstractValue vertical,

            AbstractValue width,
            AbstractValue height) : base(hAlign, horizontal, vAlign, vertical, width, height)
        {
            Texture = texture;
        }

        public override void Render(SpriteBatch spriteBatch, FieldFontRenderer fieldFontRenderer)
        {
            if(!Hidden)
            {
                Vector2 scale = (BottomRight - TopLeft) / _bounds;
                /*spriteBatch.Draw(Texture,
                    TopLeft,
                    null,
                    TintColor,
                    0,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0);*/
                spriteBatch.Draw(Texture, TopLeft, TintColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }

        public override bool Handle(IEvent evt)
        {
            return false;
        }
    }
}
