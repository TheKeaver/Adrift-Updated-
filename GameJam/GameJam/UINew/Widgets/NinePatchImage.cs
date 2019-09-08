using System;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.UINew.Widgets
{
    public class NinePatchImage : Widget
    {
        public NinePatchRegion2D NinePatch
        {
            get;
            set;
        }

        public NinePatchImage(NinePatchRegion2D ninePatch,

            HorizontalAlignment hAlign,
            AbstractValue horizontal,

            VerticalAlignment vAlign,
            AbstractValue vertical,

            AbstractValue width,
            AbstractValue height) : base(hAlign, horizontal, vAlign, vertical, width, height)
        {
            NinePatch = ninePatch;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                spriteBatch.Draw(NinePatch,
                    new Rectangle((int)TopLeft.X,
                        (int)TopLeft.Y,
                        (int)Width,
                        (int)Height),
                    Color.White);
            }
        }

        public override bool Handle(IEvent evt)
        {
            return false;
        }
    }
}
