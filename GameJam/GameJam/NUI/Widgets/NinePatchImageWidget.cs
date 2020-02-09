using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.NUI.Widgets
{
    public class NinePatchImageWidget : Widget
    {
        public WidgetProperty<NinePatchRegion2D> Image
        {
            get
            {
                return Properties.GetProperty<NinePatchRegion2D>("image");
            }
            set
            {
                Properties.SetProperty("image", value);
            }
        }

        public NinePatchImageWidget(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
            Properties.SetProperty("image", new FixedValue<NinePatchRegion2D>(null));

            entity.AddComponent(new NinePatchComponent());
            entity.GetComponent<NinePatchComponent>().RenderGroup = Constants.Render.RENDER_GROUP_UI;
            entity.GetComponent<NinePatchComponent>().Depth = Depth;
        }

        protected override void OnComputeProperties(Entity entity)
        {
            NinePatchComponent ninePatchComp = Entity.GetComponent<NinePatchComponent>();
            ninePatchComp.NinePatch = Image.Value;
            ninePatchComp.Bounds = new Vector2(Width.Value, Height.Value);
            ninePatchComp.Alpha = Alpha.Value;
            ninePatchComp.Color = Tint.Value;
        }
    }
}
