using Audrey;
using GameJam.Components;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Entities
{
    public static class ChasingEnemyEntity
    {
        private static Vector2[] GetPoints()
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
                new PolyRenderShape(GetPoints(), 0.4f, CVars.Get<Color>("color_chasing_enemy"), PolyRenderShape.PolyCapStyle.Filled, true)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_chasing_enemy")));

            entity.GetComponent<TransformComponent>().SetPosition(position);
            entity.GetComponent<TransformComponent>().SetRotation(angle);
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("chasing_enemy_size"), true);

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
        }
    }
}
