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
            entity.AddComponent(new SpriteComponent(texture, Constants.ObjectBounds.PLAYER_SHIELD_BOUNDS));
            entity.AddComponent(new PlayerShieldComponent(shipEntity));

            return entity;
        }
    }
}
