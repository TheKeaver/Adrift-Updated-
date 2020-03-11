using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public class SuperShieldDisplayEntity 
    {
        public static Entity Create(Engine engine, Entity shipEntity)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(shipEntity.GetComponent<TransformComponent>().Position));
            entity.AddComponent(new VectorSpriteComponent(
                new RenderShape[]
                {
                    new QuadRenderShape(new Vector2(3,0),
                        new Vector2(0,3),
                        new Vector2(-3,0),
                        new Vector2(0,-3),
                        shipEntity.GetComponent<ColoredExplosionComponent>().Color)
                }));

            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_NO_GLOW;
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("player_ship_size"), true);

            return entity;
        }
    }
}
