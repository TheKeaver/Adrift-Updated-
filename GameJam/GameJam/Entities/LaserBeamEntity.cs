using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public class LaserBeamEntity
    {
        public static Entity Create(Engine engine, Vector2 position, bool includeCollision)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new LaserBeamComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(10, -10),
                    new Vector2(10, 10),
                    new Vector2(-10, 10),
                    new Vector2(-10, -10), CVars.Get<Color>("color_laser_beam"))
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;

            if (includeCollision)
            {
                entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                    new Vector2(10, -10),
                    new Vector2(10, 10),
                    new Vector2(-10, 10),
                    new Vector2(-10, -10)
                })));
                entity.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_ENEMIES;
            }

            return entity;
        }
    }
}
