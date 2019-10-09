using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class PlayerShieldEntity
    {
        public static Entity Create(Engine engine, Entity shipEntity)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent());
            entity.AddComponent(new PlayerShieldComponent(shipEntity));

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(6, -1),
                    new Vector2(6, 1),
                    new Vector2(-6, 1),
                    new Vector2(-6, -1),
                    CVars.Get<Color>("color_player_shield"))
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("player_shield_size"), true);

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(6, -1),
                new Vector2(6, 1),
                new Vector2(-6, 1),
                new Vector2(-6, -1)
            })));

            return entity;
        }
    }
}
