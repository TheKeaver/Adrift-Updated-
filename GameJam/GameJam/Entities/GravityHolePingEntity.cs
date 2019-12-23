using Audrey;
using GameJam.Components;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework;

namespace GameJam.Entities
{
    public static class GravityHolePingEntity
    {
        public static Entity Create(Engine engine, ProcessManager processManager,
            Vector2 position, float radius, Entity owner = null)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                PolyRenderShape.GenerateCircleRenderShape(1f,
                    radius,
                    CVars.Get<Color>("color_gravity_hole_enemy"),
                    45)
            }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;

            entity.GetComponent<TransformComponent>().ChangeScale(radius, true);

            entity.AddComponent(new GravityHolePingComponent(owner));

            processManager.Attach(new EntityScaleProcess(engine, entity,
                CVars.Get<float>("gravity_hole_animation_ping_duration"),
                1, 0, Easings.Functions.Linear))
                .SetNext(new EntityDestructionProcess(engine, entity));

            return entity;
        }
    }
}
