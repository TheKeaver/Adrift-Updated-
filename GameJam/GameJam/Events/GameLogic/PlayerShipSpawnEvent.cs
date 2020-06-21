using Audrey;
using Events;
using Microsoft.Xna.Framework;

namespace GameJam.Events.GameLogic
{
    class PlayerShipSpawnEvent : IEvent
    {
        public PlayerShipSpawnEvent(Entity playerShipEntity, Vector2 position)
        {
            PlayerShipEntity = playerShipEntity;
            Position = position;
        }

        public Entity PlayerShipEntity
        {
            get;
            private set;
        }
        public Vector2 Position
        {
            get;
            private set;
        }
    }
}
