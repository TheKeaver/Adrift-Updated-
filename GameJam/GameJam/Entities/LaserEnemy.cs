using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class LaserEnemy
    {
        public static Entity Create(Engine engine, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new RotationComponent(CVars.Get<float>("shooting_enemy_rotational_speed")/4));
            entity.AddComponent(new MovementComponent(new Vector2(0, 1), 0));
            entity.AddComponent(new LaserEnemyComponent());
            entity.AddComponent(new EnemyComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[]{ new Vector2(4, 0),
                    new Vector2(3, 1),
                    new Vector2(-3, 3),
                    new Vector2(-5, 1),
                    new Vector2(-5, -1),
                    new Vector2(-3, -3),
                    new Vector2(3, -1)
                    }, 0.35f, Color.Gold, PolyRenderShape.PolyCapStyle.Filled, true),
                new PolyRenderShape(new Vector2[]{ new Vector2(-3, 3),
                    new Vector2(2, 0),
                    new Vector2(-3, -3)
                    }, 0.2f, Color.Gold, PolyRenderShape.PolyCapStyle.Filled)
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(/*CVars.Get<float>("kamikaze_size")*/4f, true);
            entity.AddComponent(new ColoredExplosionComponent(Color.Gold));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(4, 0),
                new Vector2(-3, 3),
                new Vector2(-5, 1),
                new Vector2(-5, -1),
                new Vector2(-3, -3)
            })));

            return entity;
        }
    }
}
