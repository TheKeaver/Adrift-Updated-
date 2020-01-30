using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.NUI.Widgets
{
    public class ImageWidget : Widget
    {
        public TextureRegion2D Texture
        {
            get
            {
                return Entity.GetComponent<SpriteComponent>().Texture;
            }
            set
            {
                Entity.GetComponent<SpriteComponent>().Texture = value;
                ComputeProperties();
            }
        }

        public ImageWidget(Engine engine) : base(engine)
        {
        }

        protected override void Initialize(Entity entity)
        {
            entity.AddComponent(new SpriteComponent());
            entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_UI;
        }

        protected override void OnComputeProperties(Entity entity)
        {
            SpriteComponent spriteComp = Entity.GetComponent<SpriteComponent>();
            spriteComp.Bounds = new Vector2(Width.Value, Height.Value);
            spriteComp.Alpha = Alpha.Value;
            spriteComp.Color = Tint;
        }
    }
}
