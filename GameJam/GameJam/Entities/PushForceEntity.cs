using Audrey;
using GameJam.Components;
using GameJam.Processes.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Entities
{
    public static class PushForceEntity
    {
        public static Entity Create(Engine engine, ProcessManager processManager, ContentManager contentManager,
            Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));

            entity.AddComponent(new SpriteComponent(new TextureRegion2D(contentManager.Load<Texture2D>("texture_gravity_hole_circle")),
                                new Vector2(2 * CVars.Get<float>("gravity_hole_enemy_radius"), 2 * CVars.Get<float>("gravity_hole_enemy_radius"))));
            entity.GetComponent<SpriteComponent>().Color = Color.Orange;

            entity.GetComponent<TransformComponent>().SetScale(0.2f, true);

            processManager.Attach(new EntityScaleProcess(engine, entity,
                0.3f,
                0.2f, 2.0f, Easings.Functions.Linear))
                .SetNext(new EntityDestructionProcess(engine, entity));
            processManager.Attach(new SpriteEntityFadeOutProcess(engine, entity,
                0.3f, Easings.Functions.SineEaseOut));

            return entity;
        }
    }
}
