using Audrey;
using GameJam.Components;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Entities
{
    public static class GravityHoleEntity
    {
        public static Entity CreateSpriteOnly(Engine engine)
        {
            return CreateSpriteOnly(engine, Vector2.Zero);
        }
        public static Entity CreateSpriteOnly(Engine engine, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new PolyRenderShape(new Vector2[] {
                    new Vector2(0,6),
                    new Vector2(-6,6),
                    new Vector2(-6,-6),
                    new Vector2(6,-6),
                    new Vector2(6,6),
                    new Vector2(0,6), // outer square end, inner begin
                    new Vector2(-6,0),
                    new Vector2(0,-6),
                    new Vector2(6,0),
                    new Vector2(0,6),
                    new Vector2(3,3), // inner square end, next begin
                    new Vector2(-3,3),
                    new Vector2(-3,-3),
                    new Vector2(3,-3),
                    new Vector2(3,3),
                    new Vector2(0,3), // next end, last begin
                    new Vector2(-3,0),
                    new Vector2(0,-3),
                    new Vector2(3,0),
                    new Vector2(0,3)
                }, 0.3f, CVars.Get<Color>("color_gravity_hole_enemy"), PolyRenderShape.PolyCapStyle.Filled, false)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<VectorSpriteComponent>().Depth = Constants.Render.RENDER_DEPTH_LAYER_SPRITES_GAMEPLAY;

            entity.GetComponent<TransformComponent>().SetScale(CVars.Get<float>("gravity_enemy_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_gravity_hole_enemy")));

            return entity;
        }

        public static Entity AddBehavior(Engine engine, Entity entity, ProcessManager processManager)
        {
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new GravityHoleEnemyComponent(
                    CVars.Get<float>("gravity_hole_enemy_radius"),
                    CVars.Get<float>("gravity_hole_enemy_force"),
                    CVars.Get<int>("gravity_hole_enemy_lifespan")));

            Process process = new WaitProcess(CVars.Get<int>("gravity_hole_enemy_lifespan"));
            processManager.Attach(process)
                .SetNext(new DelegateProcess(() => {
                    entity.GetComponent<GravityHoleEnemyComponent>().ScalingAnimation = false;
                    entity.GetComponent<GravityHoleEnemyComponent>().PingAnimation = false;
                }))
                .SetNext(new EntityScaleProcess(engine, entity,
                    CVars.Get<float>("gravity_hole_animation_despawn_duration"),
                    entity.GetComponent<TransformComponent>().Scale, 0, Easings.Functions.SineEaseIn))
                .SetNext(new EntityDestructionProcess(engine, entity));
            entity.AddComponent(new GravityHoleSpawningProcessComponent(process));

            return entity;
        }

        public static Entity Create(Engine engine, Vector2 position, ProcessManager processManager)
        {
            Entity entity = CreateSpriteOnly(engine, position);
            entity = AddBehavior(engine, entity, processManager);
            return entity;
        }

        public static void Spawn(Engine engine, ProcessManager processManager, Vector2 position)
        {
            Entity gravityHoleEntity = GravityHoleEntity.CreateSpriteOnly(engine, position);
            // Use CVars.Get<float>("gravity_enemy_size") * CVars.Get<float>("gravity_hole_animation_size_multiplier_max")
            // since the animation uses Cos
            processManager.Attach(new EntityScaleProcess(engine, gravityHoleEntity, CVars.Get<float>("gravity_hole_animation_spawn_duration"),
                0, CVars.Get<float>("gravity_enemy_size") * CVars.Get<float>("gravity_hole_animation_size_multiplier_max"), Easings.Functions.SineEaseOut))
                .SetNext(new DelegateProcess(() =>
                {
                    AddBehavior(engine, gravityHoleEntity, processManager);
                }));
            processManager.Attach(new EntityRotateProcess(engine, gravityHoleEntity, CVars.Get<float>("gravity_hole_animation_spawn_duration"),
                0, CVars.Get<float>("gravity_hole_animation_rotation_speed") * CVars.Get<float>("gravity_hole_animation_spawn_duration")));
        }
    }
}
