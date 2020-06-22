using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Entities
{
    public static class ChasingEnemyEntity
    {
        public static Vector2[] GetPoints()
        {
            return new Vector2[]{ new Vector2(3, 0),
                new Vector2(-5, 3),
                new Vector2(-4, 1),
                new Vector2(-5, 0),
                new Vector2(-4, -1),
                new Vector2(-5, -3)
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
                new PolyRenderShape(GetPoints(), 0.2f, CVars.Get<Color>("color_chasing_enemy"), PolyRenderShape.PolyCapStyle.Filled, true)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_chasing_enemy")));

            entity.GetComponent<TransformComponent>().SetPosition(position, true);
            entity.GetComponent<TransformComponent>().SetRotation(angle, true);
            entity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("chasing_enemy_size"), true);

            return entity;
        }

        public static Entity Create(Engine engine, Vector2 position, float angle)
        {
            Entity entity = CreateSpriteOnly(engine, position, angle);
            AddBehavior(entity);
            return entity;
        }

        public static Entity AddBehavior(Entity entity)
        {
            entity.AddComponent(new RotationComponent(CVars.Get<float>("chasing_enemy_rotational_speed")));
            float angle = entity.GetComponent<TransformComponent>().Rotation;
            entity.AddComponent(new MovementComponent(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), CVars.Get<float>("chasing_enemy_speed")));
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new ChasingEnemyComponent());
            entity.AddComponent(new BounceComponent());
            entity.AddComponent(new QuadTreeReferenceComponent(new QuadTreeNode(new BoundingRect())));

            entity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                new Vector2(3, 0),
                new Vector2(-5, 3),
                new Vector2(-5, -3)
            })));
            entity.GetComponent<CollisionComponent>().CollisionGroup = Constants.Collision.COLLISION_GROUP_ENEMIES;

            return entity;
        }

        public static void Spawn(Engine engine, ProcessManager processManager, Vector2 position, float angle)
        {
            Entity entity = CreateSpriteOnly(engine, position, angle);
            processManager.Attach(new EntityScaleProcess(engine,
                entity,
                1,
                0,
                CVars.Get<float>("chasing_enemy_size"),
                Easings.Functions.SineEaseOut)).SetNext(new DelegateProcess(() =>
                {
                    AddBehavior(entity);
                }));


            // Warp aniamtion. Doesn't look as good for chasing enemies since they move immediately after spawning.
            //float timeScale = 0.8f;

            //Vector2 behind = position - new Vector2(400 * (float)Math.Cos(angle), 400 * (float)Math.Sin(angle));

            //Console.WriteLine(string.Format("Attempting to warp from ({0}, {1}) to ({2}, {3}) (angle: {4})", behind.X, behind.Y, position.X, position.Y, angle));

            //for (int i = 0; i < GetPoints().Length; i++)
            //{
            //    Vector2 point = GetPoints()[i] * CVars.Get<float>("chasing_enemy_size");
            //    float cos = (float)Math.Cos(angle);
            //    float sin = (float)Math.Sin(angle);
            //    Vector2 transformedPoint = new Vector2(point.X * cos - point.Y * sin,
            //        point.X * sin + point.Y * cos);

            //    Entity warpEntity = engine.CreateEntity();
            //    warpEntity.AddComponent(new TransformComponent());
            //    warpEntity.GetComponent<TransformComponent>().SetPosition(behind + transformedPoint);
            //    warpEntity.GetComponent<TransformComponent>().SetRotation(angle);
            //    warpEntity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
            //        new PolyRenderShape(new Vector2[]
            //        {
            //            new Vector2(0, 0),
            //            new Vector2(1 / CVars.Get<float>("chasing_enemy_size"), 0)
            //        }, 0.4f,
            //        new Color[] {
            //            new Color(CVars.Get<Color>("color_chasing_enemy"), 0),
            //            CVars.Get<Color>("color_chasing_enemy")
            //        },
            //        PolyRenderShape.PolyCapStyle.None,
            //        false)
            //    }));
            //    warpEntity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("chasing_enemy_size"), true);
            //    Vector2 warpTo = position + transformedPoint;
            //    processManager.Attach(new WarpPointPhase1Process(engine, warpEntity, warpTo, 0.5f * timeScale))
            //        .SetNext(new WarpPointPhase2Process(engine, warpEntity, warpTo, 0.15f * timeScale))
            //        .SetNext(new EntityDestructionProcess(engine, warpEntity));
            //}
            //Entity actualWarpEntity = CreateSpriteOnly(engine, behind, angle);
            //actualWarpEntity.GetComponent<VectorSpriteComponent>().Alpha = 0;
            //processManager.Attach(new WaitProcess(0.5f * timeScale))
            //    .SetNext(new WarpEntityPhase2Process(engine, actualWarpEntity, position, 0.15f * timeScale))
            //    .SetNext(new DelegateProcess(() =>
            //    {
            //        AddBehavior(actualWarpEntity);
            //    }));
        }
    }
}
