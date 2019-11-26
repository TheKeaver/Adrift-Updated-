using Audrey;
using Events;

namespace GameJam.Events.EnemyActions
{
    public class LaserBeamWarmUpEnd : IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public LaserBeamWarmUpEnd(Entity entity)
        {
            Entity = entity;
        }
    }
}
