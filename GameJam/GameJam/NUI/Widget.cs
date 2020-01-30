using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.NUI
{
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }

    public abstract class Widget
    {
        WeakReference<Widget> _parent = new WeakReference<Widget>(null);
        public Widget Parent
        {
            get
            {
                Widget parent;
                if(_parent.TryGetTarget(out parent))
                {
                    return parent;
                }
                return null;
            }
            internal set
            {
                _parent = new WeakReference<Widget>(value);
            }
        }

        public Root Root
        {
            get
            {
                if(Parent == null && this is Root)
                {
                    return (Root)this;
                } else if(Parent != null)
                {
                    return Parent.Root;
                }
                return null;
            }
        }

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _horizontalAlignment;
            }
            set
            {
                _horizontalAlignment = value;
                ComputeProperties();
            }
        }
        private VerticalAlignment _verticalAlignment = VerticalAlignment.Center;
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return _verticalAlignment;
            }
            set
            {
                _verticalAlignment = value;
                ComputeProperties();
            }
        }

        private AbstractValue _marginLeft = new FixedValue(0);
        public AbstractValue MarginLeft
        {
            get
            {
                return _marginLeft;
            }
            set
            {
                _marginLeft = value;
                ComputeProperties();
            }
        }
        private AbstractValue _marginRight = new FixedValue(0);
        public AbstractValue MarginRight
        {
            get
            {
                return _marginRight;
            }
            set
            {
                _marginRight = value;
                ComputeProperties();
            }
        }
        private AbstractValue _marginTop = new FixedValue(0);
        public AbstractValue MarginTop
        {
            get
            {
                return _marginTop;
            }
            set
            {
                _marginTop = value;
                ComputeProperties();
            }
        }
        private AbstractValue _marginBottom = new FixedValue(0);
        public AbstractValue MarginBottom
        {
            get
            {
                return _marginBottom;
            }
            set
            {
                _marginBottom = value;
                ComputeProperties();
            }
        }
        public AbstractValue Margin
        {
            set
            {
                _marginLeft = value;
                _marginRight = value;
                _marginTop = value;
                _marginBottom = value;
                ComputeProperties();
            }
        }

        private AbstractValue _paddingLeft = new FixedValue(0);
        public AbstractValue PaddingLeft
        {
            get
            {
                return _paddingLeft;
            }
            set
            {
                _paddingLeft = value;
                ComputeProperties();
            }
        }
        private AbstractValue _paddingRight = new FixedValue(0);
        public AbstractValue PaddingRight
        {
            get
            {
                return _paddingRight;
            }
            set
            {
                _paddingRight = value;
                ComputeProperties();
            }
        }
        private AbstractValue _paddingTop = new FixedValue(0);
        public AbstractValue PaddingTop
        {
            get
            {
                return _paddingTop;
            }
            set
            {
                _paddingTop = value;
                ComputeProperties();
            }
        }
        private AbstractValue _paddingBottom = new FixedValue(0);
        public AbstractValue PaddingBottom
        {
            get
            {
                return _paddingBottom;
            }
            set
            {
                _paddingBottom = value;
                ComputeProperties();
            }
        }
        public AbstractValue Padding
        {
            set
            {
                _paddingLeft = value;
                _paddingRight = value;
                _paddingTop = value;
                _paddingBottom = value;
                ComputeProperties();
            }
        }

        private AbstractValue _width = new FixedValue(0);
        public AbstractValue Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                ComputeProperties();
            }
        }
        private AbstractValue _height = new FixedValue(0);
        public AbstractValue Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                ComputeProperties();
            }
        }

        private AbstractValue _alpha = new FixedValue(1);
        public AbstractValue Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
                ComputeProperties();
            }
        }

        private Color _tint = Color.White;
        public Color Tint
        {
            get
            {
                return _tint;
            }
            set
            {
                _tint = value;
            }
        }

        public Vector2 TopRight
        {
            get;
            internal set;
        } = Vector2.Zero;
        public Vector2 BottomLeft
        {
            get;
            internal set;
        } = Vector2.Zero;
        public Vector2 WorldPosition
        {
            get
            {
                return (TopRight - BottomLeft) / 2 + BottomLeft;
            }
        }

        private bool _hidden = false;
        public bool Hidden
        {
            get
            {
                return _hidden;
            }
            set
            {
                _hidden = value;
                ComputeProperties();
            }
        }

        public Engine Engine
        {
            get;
            private set;
        }

        public Entity Entity
        {
            get;
            private set;
        }

        public Widget(Engine engine)
        {
            Engine = engine;

            Entity = Engine.CreateEntity();
            Entity.AddComponent(new TransformComponent());
            

            Initialize(Entity);
        }
        ~Widget()
        {
            Engine.DestroyEntity(Entity);
        }

        protected abstract void Initialize(Entity entity);

        public void ComputeProperties()
        {
            if(Parent == null && !(this is Root))
            {
                // Don't compute properties yet; we're not fully initialized
                return;
            }
            if (this is Root)
            {
                // Root; only sets based on offset width and height
                TopRight = new Vector2(CVars.Get<float>("screen_width") / 2, CVars.Get<float>("screen_height") / 2);
                BottomLeft = new Vector2(-CVars.Get<float>("screen_width") / 2, -CVars.Get<float>("screen_height") / 2);
            } else
            {
                float x;
                switch(HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        x = Parent.BottomLeft.X + Parent.PaddingLeft.Value + MarginLeft.Value + Width.Value / 2;
                        break;
                    case HorizontalAlignment.Right:
                        x = Parent.TopRight.X - Parent.PaddingRight.Value - MarginRight.Value - Width.Value / 2;
                        break;
                    case HorizontalAlignment.Center:
                    default:
                        x = (Parent.TopRight.X - Parent.BottomLeft.X - Parent.PaddingLeft.Value - Parent.PaddingRight.Value - MarginLeft.Value - MarginRight.Value) / 2 + Parent.PaddingLeft.Value + MarginLeft.Value + Parent.BottomLeft.X;
                        break;
                }
                float y;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        y = Parent.TopRight.Y - Parent.PaddingTop.Value - MarginTop.Value - Height.Value / 2;
                        break;
                    case VerticalAlignment.Bottom:
                        y = Parent.BottomLeft.Y + Parent.PaddingBottom.Value + MarginBottom.Value + Height.Value / 2;
                        break;
                    case VerticalAlignment.Center:
                    default:
                        y = (Parent.TopRight.Y - Parent.BottomLeft.Y - Parent.PaddingTop.Value - Parent.PaddingBottom.Value - MarginTop.Value - MarginBottom.Value) / 2 + Parent.PaddingBottom.Value + MarginBottom.Value + Parent.BottomLeft.Y;
                        break;
                }

                BottomLeft = new Vector2(x - Width.Value / 2, y - Height.Value / 2);
                TopRight = new Vector2(x + Width.Value / 2, y + Height.Value / 2);

                Entity.GetComponent<TransformComponent>().Move(WorldPosition - Entity.GetComponent<TransformComponent>().Position);
            }

            OnComputeProperties(Entity);
        }
        protected abstract void OnComputeProperties(Entity entity);
    }
}
