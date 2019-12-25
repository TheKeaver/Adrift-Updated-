using Audrey;
using GameJam.Components;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Entities
{
    public static class LaserEnemyEntity
    {
        public static Entity CreateSpriteOnly(Engine engine)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent());
            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[]{ new Vector2(4, 0),
                    new Vector2(3, 1),
                    new Vector2(-3, 3),
                    new Vector2(-5, 1),
                    new Vector2(-5, -1),
                    new Vector2(-3, -3),
                    new Vector2(3, -1)
                    }, 0.35f, CVars.Get<Color>("color_laser_enemy"), PolyRenderShape.PolyCapStyle.Filled, true),
                new PolyRenderShape(new Vector2[]{ new Vector2(-3, 3),
                    new Vector2(2, 0),
                    new Vector2(-3, -3)
                    }, 0.2f, CVars.Get<Color>("color_laser_enemy"), PolyRenderShape.PolyCapStyle.Filled)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("laser_enemy_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_laser_enemy")));

            return entity;
        }

        public static Entity Create(Engine engine, ProcessManager processManager, Vector2 position, float angle)
        {
            Entity entity = CreateSpriteOnly(engine);

            entity.GetComponent<TransformComponent>().SetPosition(position);
            entity.GetComponent<TransformComponent>().SetRotation(angle);
            entity.AddComponent(new RotationComponent(CVars.Get<float>("laser_enemy_rotational_speed")));
            entity.AddComponent(new MovementComponent(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), 0));
            entity.AddComponent(new LaserEnemyComponent());
            entity.AddComponent(new EnemyComponent());

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
            Create(engine, processManager, position, angle);
        }
    }
}
