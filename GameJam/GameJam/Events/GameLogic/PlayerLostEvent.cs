using Audrey;
using Events;

namespace GameJam.Events.GameLogic
{
    public class PlayerLostEvent : IEvent
    {
        public Player Player;
        public Entity ResponsibleEntity;

        public PlayerLostEvent(Player player, Entity responsibleEntity)
        {
            Player = player;
            ResponsibleEntity = responsibleEntity;
        }
    }
}
