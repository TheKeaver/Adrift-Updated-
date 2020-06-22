using Audrey;
using Events;

namespace GameJam.Events.EnemyActions
{
    public class LaserBeamFireEnd : IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public LaserBeamFireEnd(Entity entity)
        {
            Entity = entity;
        }
    }
}
