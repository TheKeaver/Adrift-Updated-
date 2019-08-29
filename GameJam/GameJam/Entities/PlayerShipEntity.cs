using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Entities
{
    public static class PlayerShipEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new SpriteComponent(texture, Constants.ObjectBounds.PLAYER_SHIP_BOUNDS));
            entity.AddComponent(new MovementComponent());
            entity.AddComponent(new PlayerShipComponent(Constants.GamePlay.PLAYER_SHIP_MAX_HEALTH));
            entity.AddComponent(new BounceComponent());
            entity.AddComponent(new CollisionComponent(new BoundingRect(0, 0, Constants.ObjectBounds.PLAYER_SHIP_BOUNDS.X, Constants.ObjectBounds.PLAYER_SHIP_BOUNDS.Y)));

            entity.GetComponent<MovementComponent>().UpdateRotationWithDirection = false;

            return entity;
        }
    }
}
