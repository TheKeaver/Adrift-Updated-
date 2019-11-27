using Audrey;
using Events;

namespace GameJam.Events.GameLogic
{
    public class GameOverEvent : IEvent
    {
        public Player Player;
        public Entity ResponsibleEntity;

        public GameOverEvent(Player player, Entity responsibleEntity)
        {
            Player = player;
            ResponsibleEntity = responsibleEntity;
        }
    }
}
