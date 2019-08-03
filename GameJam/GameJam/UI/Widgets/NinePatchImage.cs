using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.UI.Widgets
{
    /// <summary>
    /// A UI widget for a nine-patch image.
    /// </summary>
    public class NinePatchImage : Widget
    {
        readonly NinePatchRegion2D _ninePatch;

        public NinePatchImage(NinePatchRegion2D ninePatch,
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
            _ninePatch = ninePatch;
        }

        public NinePatchImage(NinePatchRegion2D ninePatch,
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
            _ninePatch = ninePatch;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Hidden)
            {
                spriteBatch.Draw(_ninePatch,
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
