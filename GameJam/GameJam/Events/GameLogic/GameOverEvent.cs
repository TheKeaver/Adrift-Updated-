using Audrey;
using Events;

namespace GameJam.Events.GameLogic
{
    public class GameOverEvent : IEvent
    {
        public Entity ShipShield;

        public GameOverEvent(Entity shield)
        {
            ShipShield = shield;
        }
    }
}
