using Audrey;
using GameJam.Components;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class ShootingEnemyEntity
    {
        public static Entity Create(Engine engine, Vector2 position, ProcessManager processManager)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new ShootingEnemyComponent(CVars.Get<int>("shooting_enemy_projectile_ammo")));
            entity.AddComponent(new RotationComponent(CVars.Get<float>("shooting_enemy_rotational_speed")));
            entity.AddComponent(new MovementComponent(new Vector2(0, 1), CVars.Get<float>("shooting_enemy_speed")));
            entity.AddComponent(new EnemyComponent());

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[]{ new Vector2(-2, -5),
                    new Vector2(-2, 5),
                    new Vector2(2, 4),
                    new Vector2(1, 1),
                    new Vector2(4, 0),
                    new Vector2(1, -1),
                    new Vector2(2, -4)
                    }, 0.3f, CVars.Get<Color>("color_shooting_enemy"), PolyRenderShape.PolyCapStyle.Filled, true)
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("shooting_enemy_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_shooting_enemy")));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(-2, 5),
                new Vector2(2, 4),
                new Vector2(4, 0),
                new Vector2(2, -4),
                new Vector2(-2, -5)
            })));

            FireProjectileProcess fpp = new FireProjectileProcess(entity, engine);
            processManager.Attach(fpp);
            entity.AddComponent(new ProjectileSpawningProcessComponent(fpp));

            return entity;
        }
    }
}
