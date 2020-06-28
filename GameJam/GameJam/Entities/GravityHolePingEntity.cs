using Audrey;
using GameJam.Components;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Entities
{
    public static class GravityHolePingEntity
    {
        public static Entity Create(Engine engine, ProcessManager processManager, ContentManager contentManager,
            Vector2 position, float radius, Entity owner = null)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));

            /*entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                PolyRenderShape.GenerateCircleRenderShape(1f,
                    radius,
                    CVars.Get<Color>("color_gravity_hole_enemy"),
                    45)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<VectorSpriteComponent>().Depth = Constants.Render.RENDER_DEPTH_LAYER_SPRITES_GAMEPLAY;
            */

            entity.AddComponent(new SpriteComponent(new TextureRegion2D(contentManager.Load<Texture2D>("texture_gravity_hole_circle")),
                                new Vector2(2*CVars.Get<float>("gravity_hole_enemy_radius"), 2*CVars.Get<float>("gravity_hole_enemy_radius"))));
            entity.GetComponent<SpriteComponent>().Color = CVars.Get<Color>("color_gravity_hole_enemy");

            entity.GetComponent<TransformComponent>().SetScale(radius, true);

            entity.AddComponent(new GravityHolePingComponent(owner));

            processManager.Attach(new EntityScaleProcess(engine, entity,
                CVars.Get<float>("gravity_hole_animation_ping_duration"),
                1, 0, Easings.Functions.Linear))
                .SetNext(new EntityDestructionProcess(engine, entity));

            return entity;
        }
    }
}
