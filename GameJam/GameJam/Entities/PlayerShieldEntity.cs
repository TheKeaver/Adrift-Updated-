using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Entities
{
    public static class PlayerShieldEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Entity shipEntity)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent());
            // TODO: Re-implement shield
            entity.AddComponent(new PlayerShieldComponent(shipEntity));

            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                new QuadRenderShape(new Vector2(6, -1),
                    new Vector2(6, 1),
                    new Vector2(-6, 1),
                    new Vector2(-6, -1),
                    Color.SpringGreen)
            }));
            entity.GetComponent<TransformComponent>().ChangeScale(CVars.Get<float>("player_shield_size"), true);

            return entity;
        }
    }
}
