using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Entities
{
    public static class KamikazeEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new RotationComponent(CVars.Get<float>("kamikaze_enemy_rotational_speed")));
            entity.AddComponent(new MovementComponent(new Vector2(0,1), CVars.Get<float>("kamikaze_enemy_speed")));
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new KamikazeComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[]{ new Vector2(3, 0),
                    new Vector2(-5, 3),
                    new Vector2(-4, 1),
                    new Vector2(-5, 0),
                    new Vector2(-4, -1),
                    new Vector2(-5, -3)
                    }, 0.4f, Color.Violet, PolyRenderShape.PolyCapStyle.Filled, true)
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("kamikaze_size"), true);

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(5, 0),
                new Vector2(-4, 3),
                new Vector2(-4, -3)
            })));

            return entity;
        }
    }
}
