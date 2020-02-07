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
    public static class LaserEnemyEntity
    {
        private static Vector2[] GetOuterPoints()
        {
            return new Vector2[]{ new Vector2(4, 0),
                new Vector2(3, 1),
                new Vector2(-3, 3),
                new Vector2(-5, 1),
                new Vector2(-5, -1),
                new Vector2(-3, -3),
                new Vector2(3, -1)
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
                new PolyRenderShape(GetOuterPoints(), 0.35f, CVars.Get<Color>("color_laser_enemy"), PolyRenderShape.PolyCapStyle.Filled, true),
                new PolyRenderShape(new Vector2[]{ new Vector2(-3, 3),
                    new Vector2(2, 0),
                    new Vector2(-3, -3)
                    }, 0.2f, CVars.Get<Color>("color_laser_enemy"), PolyRenderShape.PolyCapStyle.Filled)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES; entity.GetComponent<TransformComponent>().SetPosition(position);
            entity.GetComponent<TransformComponent>().SetRotation(angle);
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("laser_enemy_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_laser_enemy")));

            return entity;
        }

        public static Entity Create(Engine engine, ProcessManager processManager, Vector2 position, float angle)
        {
            Entity entity = CreateSpriteOnly(engine, position, angle);
            return AddBehavior(engine, entity, processManager);
        }

        public static Entity AddBehavior(Engine engine, Entity entity, ProcessManager processManager)
        {
            entity.AddComponent(new RotationComponent(CVars.Get<float>("laser_enemy_rotational_speed")));
            float angle = entity.GetComponent<TransformComponent>().Rotation;
            entity.AddComponent(new MovementComponent(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), 0));
            entity.AddComponent(new LaserEnemyComponent());
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new QuadTreeReferenceComponent(new QuadTreeNode(new BoundingRect())));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(4, 0),
                new Vector2(-3, 3),
                new Vector2(-5, 1),
                new Vector2(-5, -1),
                new Vector2(-3, -3)
            })));
            entity.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_ENEMIES;

            processManager.Attach(CreateLaserEnemyProcessChain(processManager, engine, entity, CVars.Get<float>("laser_enemy_spawn_wait_period"), true));

            return entity;
        }

        public static Process CreateLaserEnemyProcessChain(ProcessManager processManager, Engine engine, Entity entity, float waitTime, bool loop)
        {
            Process chain = new WaitProcess(waitTime);
            Process loopProcess = null;
            if (loop)
            {
                loopProcess = new DelegateProcess(() =>
                {
                    processManager.Attach(CreateLaserEnemyProcessChain(processManager, engine, entity, CVars.Get<float>("laser_enemy_successive_wait_period"), true));
                });
            }
            chain.SetNext(new LaserEnemyFreezeRotation(engine, entity))
                .SetNext(new LaserWarmUpProcess(engine, entity))
                .SetNext(new WaitProcess(CVars.Get<float>("laser_enemy_warm_up_duration")))
                .SetNext(new LaserShootProcess(engine, entity))
                .SetNext(new LaserEnemyUnfreezeRotation(engine, entity))
                .SetNext(loopProcess);
            return chain;
        }

        public static void Spawn(Engine engine, ProcessManager processManager, Vector2 position, float angle)
        {
            float timeScale = CVars.Get<float>("animation_spawn_warp_time_scale");

            Vector2 behind = position - new Vector2(CVars.Get<float>("animation_spawn_warp_distance") * (float)Math.Cos(angle),
                CVars.Get<float>("animation_spawn_warp_distance") * (float)Math.Sin(angle));

            for (int i = 0; i < GetOuterPoints().Length; i++)
            {
                Vector2 point = GetOuterPoints()[i] * CVars.Get<float>("laser_enemy_size");
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);
                Vector2 transformedPoint = new Vector2(point.X * cos - point.Y * sin,
                    point.X * sin + point.Y * cos);

                Entity warpEntity = engine.CreateEntity();
                warpEntity.AddComponent(new TransformComponent());
                warpEntity.GetComponent<TransformComponent>().SetPosition(behind + transformedPoint);
                warpEntity.GetComponent<TransformComponent>().SetRotation(angle);
                warpEntity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                    new PolyRenderShape(new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(1 / CVars.Get<float>("laser_enemy_size"), 0)
                    }, 0.3f,
                    new Color[] {
                        new Color(CVars.Get<Color>("color_laser_enemy"), 0),
                        CVars.Get<Color>("color_laser_enemy")
                    },
                    PolyRenderShape.PolyCapStyle.None,
                    false)
                }));
                warpEntity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("laser_enemy_size"), true);
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
