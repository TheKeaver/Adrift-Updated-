using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.NUI.Widgets
{
    public class ImageWidget : Widget
    {
        public WidgetProperty<TextureRegion2D> Image
        {
            get
            {
                return Properties.GetProperty<TextureRegion2D>("image");
            }
            set
            {
                Properties.SetProperty("image", value);
            }
        }

        public ImageWidget(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
            Properties.SetProperty("image", new FixedValue<TextureRegion2D>(null));

            entity.AddComponent(new SpriteComponent());
            entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_UI;
            entity.GetComponent<SpriteComponent>().Depth = Depth;
        }

        protected override void OnComputeProperties(Entity entity)
        {
            SpriteComponent spriteComp = Entity.GetComponent<SpriteComponent>();
            spriteComp.Texture = Image.Value;
            spriteComp.Bounds = new Vector2(Width.Value, Height.Value);
            spriteComp.Alpha = Alpha.Value;
            spriteComp.Color = Tint.Value;
            spriteComp.Hidden = Hidden.Value;
        }
    }
}
