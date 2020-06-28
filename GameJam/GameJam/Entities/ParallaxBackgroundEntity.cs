using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Entities
{
    public static class ParallaxBackgroundEntity
    {
        public static Entity Create(Engine engine, TextureRegion2D texture, Vector2 origin, float strength, bool pulsing = false)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(origin));
            entity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("background_stars_scale"), true);
            entity.AddComponent(new SpriteComponent(texture,
                new Vector2(texture.Width, texture.Height)));
            entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_STARS;
            entity.GetComponent<SpriteComponent>().Depth = Constants.Render.RENDER_DEPTH_LAYER_SPRITES_BACKGROUND;
            if (pulsing)
            {
                entity.AddComponent(new PulseComponent(10, 0.75f, 1));
            }
            entity.AddComponent(new ParallaxBackgroundComponent(strength, origin));

            return entity;
        }
    }
}
