using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class PlayerShipEntity
    {
        public static Entity Create(Engine engine, Vector2 position)
        {
            return Create(engine, position, Color.White);
        }

        public static Entity Create(Engine engine, Vector2 position, Color color)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));

            entity.AddComponent(new MovementComponent());
            entity.AddComponent(new PlayerShipComponent(CVars.Get<int>("player_ship_max_health")));
            entity.AddComponent(new BounceComponent());

            entity.GetComponent<MovementComponent>().UpdateRotationWithDirection = false;

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[]{ new Vector2(3, 0),
                    new Vector2(0, 3),
                    new Vector2(-3, 0),
                    new Vector2(0, -3)
                    }, 0.2f, color, PolyRenderShape.PolyCapStyle.Filled, true),
                new PolyRenderShape(new Vector2[]{ new Vector2(0, 3),
                    new Vector2(-3, -2),
                    new Vector2(-3, 2),
                    new Vector2(0, -3)
                    }, 0.2f, color, PolyRenderShape.PolyCapStyle.Filled)
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("player_ship_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(Color.White));


            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(3, 0),
                new Vector2(0, 3),
                new Vector2(-3, 2),
                new Vector2(-3, -2)
            })));

            return entity;
        }
    }
}
