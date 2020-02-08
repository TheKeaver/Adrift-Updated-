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

        public WidgetProperties Properties
        {
            get;
            private set;
        }

        public WidgetProperty<HorizontalAlignment> HAlign
        {
            get
            {
                return Properties.GetProperty<HorizontalAlignment>("halign");
            }
            set
            {
                Properties.SetProperty("halign", value);
            }
        }
        public WidgetProperty<VerticalAlignment> VAlign
        {
            get
            {
                return Properties.GetProperty<VerticalAlignment>("valign");
            }
            set
            {
                Properties.SetProperty("valign", value);
            }
        }

        public WidgetProperty<float> MarginLeft
        {
            get
            {
                return Properties.GetProperty<float>("margin-left");
            }
            set
            {
                Properties.SetProperty("margin-left", value);
            }
        }
        public WidgetProperty<float> MarginRight
        {
            get
            {
                return Properties.GetProperty<float>("margin-right");
            }
            set
            {
                Properties.SetProperty("margin-right", value);
            }
        }
        public WidgetProperty<float> MarginTop
        {
            get
            {
                return Properties.GetProperty<float>("margin-top");
            }
            set
            {
                Properties.SetProperty("margin-top", value);
            }
        }
        public WidgetProperty<float> MarginBottom
        {
            get
            {
                return Properties.GetProperty<float>("margin-bottom");
            }
            set
            {
                Properties.SetProperty("margin-bottom", value);
            }
        }
        public WidgetProperty<float> Margin
        {
            set
            {
                Properties.SetProperty("margin", value);
            }
        }

        public WidgetProperty<float> PaddingLeft
        {
            get
            {
                return Properties.GetProperty<float>("padding-left");
            }
            set
            {
                Properties.SetProperty("padding-left", value);
            }
        }
        public WidgetProperty<float> PaddingRight
        {
            get
            {
                return Properties.GetProperty<float>("padding-right");
            }
            set
            {
                Properties.SetProperty("padding-right", value);
            }
        }
        public WidgetProperty<float> PaddingTop
        {
            get
            {
                return Properties.GetProperty<float>("padding-top");
            }
            set
            {
                Properties.SetProperty("padding-top", value);
            }
        }
        public WidgetProperty<float> PaddingBottom
        {
            get
            {
                return Properties.GetProperty<float>("padding-bottom");
            }
            set
            {
                Properties.SetProperty("padding-bottom", value);
            }
        }
        public WidgetProperty<float> Padding
        {
            set
            {
                Properties.SetProperty("padding", value);
            }
        }

        public WidgetProperty<float> Width
        {
            get
            {
                return Properties.GetProperty<float>("width");
            }
            set
            {
                Properties.SetProperty("width", value);
            }
        }
        public WidgetProperty<float> Height
        {
            get
            {
                return Properties.GetProperty<float>("height");
            }
            set
            {
                Properties.SetProperty("height", value);
            }
        }

        public WidgetProperty<float> Alpha
        {
            get
            {
                return Properties.GetProperty<float>("alpha");
            }
            set
            {
                Properties.SetProperty("alpha", value);
            }
        }

        public WidgetProperty<Color> Tint
        {
            get
            {
                return Properties.GetProperty<Color>("tint");
            }
            set
            {
                Properties.SetProperty("tint", value);
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

        public bool BeginNewLayer
        {
            get;
            set;
        }
        public virtual int UiLayer
        {
            get
            {
                if(Parent == null)
                {
                    return 0;
                }
                return Parent.UiLayer + (BeginNewLayer ? 1 : 0);
            }
        }
        public virtual int LayerIndex
        {
            get
            {
                if(Parent == null)
                {
                    return 0;
                }
                if(BeginNewLayer)
                {
                    return 0;
                }
                return Parent.LayerIndex + 1;
            }
        }
        public float Depth
        {
            get
            {
                return Constants.Render.GetUIRenderDepth(UiLayer, LayerIndex);
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

            PopulateProperties();
            Initialize(Entity);
            Properties.Lock();
            Properties.ComputePropertiesOnSet = true;
        }
        ~Widget()
        {
            Engine.DestroyEntity(Entity);
        }

        private void PopulateProperties()
        {
            Properties = new WidgetProperties(ComputeProperties);
            Properties.ComputePropertiesOnSet = false;

            Properties.SetProperty("halign", new FixedValue<HorizontalAlignment>(HorizontalAlignment.Center));
            Properties.SetProperty("valign", new FixedValue<VerticalAlignment>(VerticalAlignment.Center));

            Properties.SetProperty("margin-left", new FixedValue<float>(0));
            Properties.SetProperty("margin-right", new FixedValue<float>(0));
            Properties.SetProperty("margin-top", new FixedValue<float>(0));
            Properties.SetProperty("margin-bottom", new FixedValue<float>(0));
            Properties.SetPropertyAsProxy("margin", new [] { "margin-left", "margin-right", "margin-top", "margin-bottom" });

            Properties.SetProperty("padding-left", new FixedValue<float>(0));
            Properties.SetProperty("padding-right", new FixedValue<float>(0));
            Properties.SetProperty("padding-top", new FixedValue<float>(0));
            Properties.SetProperty("padding-bottom", new FixedValue<float>(0));
            Properties.SetPropertyAsProxy("padding", new[] { "padding-left", "padding-right", "padding-top", "padding-bottom" });

            Properties.SetProperty("width", new FixedValue<float>(0));
            Properties.SetProperty("height", new FixedValue<float>(0));

            Properties.SetProperty("alpha", new FixedValue<float>(1));
            Properties.SetProperty("tint", new FixedValue<Color>(Color.White));

            Properties.SetProperty("hidden", new FixedValue<bool>(false));
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
                TopRight = new Vector2(Width.Value / 2, Height.Value / 2);
                BottomLeft = new Vector2(-Width.Value / 2, -Height.Value / 2);
            } else
            {
                float x;
                switch(HAlign.Value)
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
                switch (VAlign.Value)
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
            }

            Entity.GetComponent<TransformComponent>().Move(WorldPosition - Entity.GetComponent<TransformComponent>().Position);

            OnComputeProperties(Entity);
        }
        protected abstract void OnComputeProperties(Entity entity);
    }
}
