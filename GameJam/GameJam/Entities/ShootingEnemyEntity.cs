using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Processes.Animations.Warp;
using GameJam.Processes.Enemies;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Entities
{
    public static class ShootingEnemyEntity
    {
        private static Vector2[] GetPoints()
        {
            return new Vector2[]{
                new Vector2(-2, -5),
                new Vector2(-2, 5),
                new Vector2(2, 4),
                new Vector2(1, 1),
                new Vector2(4, 0),
                new Vector2(1, -1),
                new Vector2(2, -4)
            };
        }

        public static Entity CreateSpriteOnly(Engine engine)
        {
            return CreateSpriteOnly(engine, Vector2.Zero, 0);
        }
        public static Entity CreateSpriteOnly(Engine engine, Vector2 position, float angle)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent());
            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(GetPoints(), 0.3f, CVars.Get<Color>("color_shooting_enemy"), PolyRenderShape.PolyCapStyle.Filled, true)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("shooting_enemy_size"), true);
            entity.GetComponent<TransformComponent>().SetPosition(position, true);
            entity.GetComponent<TransformComponent>().SetRotation(angle, true);
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_shooting_enemy")));

            return entity;
        }

        public static Entity Create(Engine engine, Vector2 position, ProcessManager processManager, float angle)
        {
            Entity entity = CreateSpriteOnly(engine, position, angle);
            AddBehavior(engine, entity, processManager);
            return entity;
        }

        public static Entity AddBehavior(Engine engine, Entity entity, ProcessManager processManager)
        {
            entity.AddComponent(new ShootingEnemyComponent(CVars.Get<int>("shooting_enemy_projectile_ammo")));
            entity.AddComponent(new RotationComponent(CVars.Get<float>("shooting_enemy_rotational_speed")));
            float angle = entity.GetComponent<TransformComponent>().Rotation;
            entity.AddComponent(new MovementComponent(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), CVars.Get<float>("shooting_enemy_speed")));
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new BounceComponent());
            entity.AddComponent(new QuadTreeReferenceComponent(new QuadTreeNode(new BoundingRect())));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(-2, 5),
                new Vector2(2, 4),
                new Vector2(4, 0),
                new Vector2(2, -4),
                new Vector2(-2, -5)
            })));
            entity.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_ENEMIES;

            FireProjectileProcess fpp = new FireProjectileProcess(entity, engine);
            processManager.Attach(fpp);
            entity.AddComponent(new ProjectileSpawningProcessComponent(fpp));

            return entity;
        }

        public static void Spawn(Engine engine, ProcessManager processManager, Vector2 position, float angle)
        {
            float timeScale = CVars.Get<float>("animation_spawn_warp_time_scale");

            Vector2 behind = position - new Vector2(CVars.Get<float>("animation_spawn_warp_distance") * (float)Math.Cos(angle),
                CVars.Get<float>("animation_spawn_warp_distance") * (float)Math.Sin(angle));

            for (int i = 0; i < GetPoints().Length; i++)
            {
                Vector2 point = GetPoints()[i] * CVars.Get<float>("shooting_enemy_size");
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);
                Vector2 transformedPoint = new Vector2(point.X * cos - point.Y * sin,
                    point.X * sin + point.Y * cos);

                Entity warpEntity = engine.CreateEntity();
                warpEntity.AddComponent(new TransformComponent());
                warpEntity.GetComponent<TransformComponent>().SetPosition(behind + transformedPoint, true);
                warpEntity.GetComponent<TransformComponent>().SetRotation(angle, true);
                warpEntity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                    new PolyRenderShape(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(1 / CVars.Get<float>("shooting_enemy_size"), 0)
                    }, 0.3f,
                    new Color[] {
                        new Color(CVars.Get<Color>("color_shooting_enemy"), 0),
                        CVars.Get<Color>("color_shooting_enemy")
                    },
                    PolyRenderShape.PolyCapStyle.None,
                    false)
                }));
                warpEntity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("shooting_enemy_size"), true);
                Vector2 warpTo = position + transformedPoint;
                processManager.Attach(new WarpPointPhase1Process(engine, warpEntity, warpTo, CVars.Get<float>("animation_spawn_warp_phase_1_base_duration") * timeScale))
                    .SetNext(new WarpPointPhase2Process(engine, warpEntity, warpTo, CVars.Get<float>("animation_spawn_warp_phase_2_base_duration") * timeScale))
                    .SetNext(new EntityDestructionProcess(engine, warpEntity));
            }
            Entity actualWarpEntity = CreateSpriteOnly(engine, behind, angle);
            actualWarpEntity.GetComponent<VectorSpriteComponent>().Alpha = 0;
            processManager.Attach(new WaitProcess(CVars.Get<float>("animation_spawn_warp_phase_1_base_duration") * timeScale))
                .SetNext(new WarpEntityPhase2Process(engine, actualWarpEntity, position, CVars.Get<float>("animation_spawn_warp_phase_2_base_duration") * timeScale))
                .SetNext(new DelegateProcess(() =>
                {
                    AddBehavior(engine, actualWarpEntity, processManager);
                }));
        }
    }
}
