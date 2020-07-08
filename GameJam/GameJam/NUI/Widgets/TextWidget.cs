using Audrey;
using FontExtension;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.NUI.Widgets
{
    public class TextWidget : Widget
    {
        public WidgetProperty<FieldFont> Font
        {
            get
            {
                return Properties.GetProperty<FieldFont>("font");
            }
            set
            {
                Properties.SetProperty("font", value);
            }
        }
        public WidgetProperty<string> Content
        {
            get
            {
                return Properties.GetProperty<string>("content");
            }
            set
            {
                Properties.SetProperty("content", value);
            }
        }
        public WidgetProperty<FieldFontJustify> Justify
        {
            get
            {
                return Properties.GetProperty<FieldFontJustify>("justify");
            }
            set
            {
                Properties.SetProperty("justify", value);
            }
        }

        public float AspectRatio
        {
            get
            {
                Vector2 bounds = Font.Value.MeasureString(Content.Value);
                return bounds.X / bounds.Y;
            }
        }

        public TextWidget(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
            Properties.SetProperty("font", new FixedValue<FieldFont>(null));
            Properties.SetProperty("content", new FixedValue<string>(null));
            Properties.SetProperty("justify", new FixedValue<FieldFontJustify>(FieldFontJustify.Center));

            Properties.SetPropertyReaction("width", OnWidthSet);
            Properties.SetPropertyReaction("height", OnHeightSet);
            Properties.SetPropertyReaction("font", UpdateAspectRatioProperty);
            Properties.SetPropertyReaction("content", UpdateAspectRatioProperty);

            entity.AddComponent(new FieldFontComponent());
            entity.GetComponent<FieldFontComponent>().RenderGroup = Constants.Render.RENDER_GROUP_UI;
        }

        protected override void OnComputeProperties(Entity entity)
        {
            FieldFontComponent fontComp = Entity.GetComponent<FieldFontComponent>();
            fontComp.Font = Font.Value;
            fontComp.Content = Content.Value;
            fontComp.Alpha = Alpha.Value;
            fontComp.Color = Tint.Value;
            fontComp.Depth = Depth;
            fontComp.Justify = Justify;

            if (!(Width is AspectRatioValue<float>) && !(Height is AspectRatioValue<float>))
            {
                // Pick one
                // We'll assume height is desired
                OnHeightSet();
            }

            Entity.GetComponent<TransformComponent>().SetScale(1);
        }

        private void OnWidthSet()
        {
            if(Font == null || Content == null)
            {
                return;
            }
            if(!(Height is AspectRatioValue<float>))
            {
                Height = new AspectRatioValue<float>(this, AspectRatio, AspectRatioValue<float>.ComputeThisFromOther.Height);
            }
        }
        private void OnHeightSet()
        {
            if(Font == null || Content == null)
            {
                return;
            }
            if (!(Width is AspectRatioValue<float>))
            {
                Width = new AspectRatioValue<float>(this, AspectRatio, AspectRatioValue<float>.ComputeThisFromOther.Width);
            }
        }

        private void UpdateAspectRatioProperty()
        {
            if (Width is AspectRatioValue<float>)
            {
                OnHeightSet();
            }
            if (Height is AspectRatioValue<float>)
            {
                OnWidthSet();
            }
        }
    }
}
