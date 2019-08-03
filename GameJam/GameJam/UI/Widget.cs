using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.UI
{
    /// <summary>
    /// Base class for all widgets.
    /// </summary>
    public abstract class Widget : IEventListener
    {
        Widget _parent = null;
        public Widget Parent
        {
            get
            {
                return _parent;
            }
            internal set
            {
                _parent = value;
                ComputeProperties();
            }
        }

        public Origin Origin
        {
            get;
            protected set;
        }

        public bool Hidden = false;

        public Vector2 TopLeft
        {
            get;
            private set;
        } = Vector2.Zero;
        public Vector2 BottomRight
        {
            get;
            private set;
        } = Vector2.Zero;

        public float Width
        {
            get
            {
                return BottomRight.X - TopLeft.X;
            }
        }
        public float Height
        {
            get
            {
                return BottomRight.Y - TopLeft.Y;
            }
        }

        public float AspectRatio
        {
            get;
            protected set;
        }
        public AspectRatioType AspectRatioType
        {
            get;
            private set;
        }

        public float PercentX
        {
            get;
            set;
        }
        public float PercentY
        {
            get;
            set;
        }
        public float POffsetX
        {
            get;
            set;
        }
        public float POffsetY
        {
            get;
            set;
        }

        public float PercentWidth
        {
            get;
            set;
        }
        public float POffsetWidth
        {
            get;
            set;
        }
        public float PercentHeight
        {
            get;
            set;
        }
        public float POffsetHeight
        {
            get;
            set;
        }

        public Widget(Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentWidth,
                      float pOffsetWidth,
                      float percentHeight,
                      float pOffsetHeight)
        {
            Origin = origin;
            PercentX = percentX;
            PercentY = percentY;
            POffsetX = pOffsetX;
            POffsetY = pOffsetY;
            PercentWidth = percentWidth;
            POffsetWidth = pOffsetWidth;
            PercentHeight = percentHeight;
            POffsetHeight = pOffsetHeight;

            AspectRatioType = AspectRatioType.None;
        }

        public Widget(Origin origin,
                      float percentX,
                      float pOffsetX,
                      float percentY,
                      float pOffsetY,
                      float percentAspect,
                      float pOffsetAspect,
                      float aspectRatio,
                      AspectRatioType aspectRatioType)
        {
            Origin = origin;
            PercentX = percentX;
            PercentY = percentY;
            POffsetX = pOffsetX;
            POffsetY = pOffsetY;

            PercentWidth = percentAspect;
            POffsetWidth = pOffsetAspect;
            PercentHeight = percentAspect;
            POffsetHeight = pOffsetAspect;

            AspectRatio = aspectRatio;

            AspectRatioType = aspectRatioType;
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public void ComputeProperties()
        {
            if (Parent == null)
            {
                // Root; only sets based on offset width and height
                TopLeft = Vector2.Zero;
                BottomRight = new Vector2(POffsetWidth, POffsetHeight);
            }
            else
            {
                float x = Parent.TopLeft.X;
                x += PercentX * Parent.Width;
                x += POffsetX;

                float y = Parent.TopLeft.Y;
                y += PercentY * Parent.Height;
                y += POffsetY;

                float w, h;
                switch (AspectRatioType)
                {
                    case AspectRatioType.WidthMaster:
                        w = Parent.Width;
                        w *= PercentWidth;
                        w += POffsetWidth;

                        h = w / AspectRatio;
                        break;
                    case AspectRatioType.HeightMaster:
                        h = Parent.Height;
                        h *= PercentHeight;
                        h += POffsetHeight;

                        w = h * AspectRatio;
                        break;
                    case AspectRatioType.None:
                    default:
                        w = Parent.Width;
                        w *= PercentWidth;
                        w += POffsetWidth;
                        h = Parent.Height;
                        h *= PercentHeight;
                        h += POffsetHeight;

                        AspectRatio = w / h;
                        break;
                }

                switch (Origin)
                {
                    case Origin.BottomRight:
                    case Origin.BottomLeft:
                        y = Parent.BottomRight.Y - (y - Parent.TopLeft.Y);
                        y -= h;

                        if (Origin == Origin.BottomRight)
                        {
                            goto case Origin.TopRight;
                        }
                        break;
                    case Origin.TopRight:
                        x = Parent.BottomRight.X - (x - Parent.TopLeft.X);
                        x -= w;
                        break;
                    case Origin.Center:
                        x = x + Parent.Width * 0.5f;
                        x -= (w * 0.5f);

                        y = y + Parent.Height * 0.5f;
                        y -= (h * 0.5f);
                        break;
                }

                TopLeft = new Vector2(x, y);
                BottomRight = new Vector2(x + w,
                                          y + h);
            }

            OnComputeProperties();
        }

        protected virtual void OnComputeProperties()
        {
        }

        public abstract bool Handle(IEvent evt);
    }

    public enum Origin
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center
    }

    public enum AspectRatioType
    {
        None,
        WidthMaster,
        HeightMaster
    }
}
