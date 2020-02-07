using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class ProjectileEntity
    {
        public static Entity Create(Engine engine, Vector2 position, Vector2 direction)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new ProjectileComponent(CVars.Get<int>("shooting_enemy_projectile_bounces"), CVars.Get<Color>("color_projectile")));
            entity.AddComponent(new BounceComponent());
            entity.AddComponent(new MovementComponent(direction, CVars.Get<float>("shooting_enemy_projectile_speed")));
            entity.AddComponent(new EnemyComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(3, -1), new Vector2(3, 1),
                    new Vector2(-3, 1), new Vector2(-3, -1),
                    CVars.Get<Color>("color_projectile"))
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("projectile_size"), false);
            entity.AddComponent(new QuadTreeReferenceComponent(new QuadTreeNode(new BoundingRect())));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(3, -1),
                new Vector2(3, 1),
                new Vector2(-3, 1),
                new Vector2(-3, -1)
            })));
            entity.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_ENEMIES;

            return entity;
        }
    }
}
