using Audrey;
using GameJam.Components;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Entities
{
    public static class ShootingEnemyEntity
    {
        public static Entity CreateSpriteOnly(Engine engine)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent());
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
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("shooting_enemy_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_shooting_enemy")));

            return entity;
        }

        public static Entity Create(Engine engine, Vector2 position, ProcessManager processManager, float angle)
        {
            Entity entity = CreateSpriteOnly(engine);

            entity.GetComponent<TransformComponent>().SetPosition(position);
            entity.GetComponent<TransformComponent>().SetRotation(angle);
            entity.AddComponent(new ShootingEnemyComponent(CVars.Get<int>("shooting_enemy_projectile_ammo")));
            entity.AddComponent(new RotationComponent(CVars.Get<float>("shooting_enemy_rotational_speed")));
            entity.AddComponent(new MovementComponent(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), CVars.Get<float>("shooting_enemy_speed")));
            entity.AddComponent(new EnemyComponent());

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
