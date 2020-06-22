using Audrey;
using Events;

namespace GameJam.Events.EnemyActions
{
    public class OutOfAmmoEvent : IEvent
    {
        public Entity ShootingEnemyOOA;
        public OutOfAmmoEvent(Entity self)
        {
            ShootingEnemyOOA = self;
        }
    }
}
