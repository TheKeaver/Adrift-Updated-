using Audrey;
using GameJam.Components;
using GameJam.Processes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameJam.Entities
{
    public static class GravityHoleEntity
    {
        public static Entity Create(Engine engine, Vector2 position, ProcessManager processManager)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new GravityHoleEnemyComponent(
                    CVars.Get<float>("gravity_hole_enemy_radius"),
                    CVars.Get<float>("gravity_hole_enemy_force"),
                    CVars.Get<int>("gravity_hole_enemy_lifespan")));

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
                }, 0.3f, CVars.Get<Color>("color_gravity_hold_enemy"), PolyRenderShape.PolyCapStyle.Filled, false)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;

            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("gravity_enemy_size"), true);
            entity.AddComponent(new ColoredExplosionComponent(CVars.Get<Color>("color_gravity_hold_enemy")));

            WaitProcess wp = new WaitProcess(CVars.Get<int>("gravity_hole_enemy_lifespan"));
            EntityDestructionProcess dp = new EntityDestructionProcess(engine, entity);
            wp.SetNext(dp);
            processManager.Attach(wp);
            entity.AddComponent(new GravityHoleSpawningProcessComponent(wp));

            Console.WriteLine("Gravity entity spawned");
            return entity;
        }
    }
}
