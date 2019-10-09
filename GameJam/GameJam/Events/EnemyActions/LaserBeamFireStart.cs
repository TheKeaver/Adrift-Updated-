using System;
using Audrey;
using Events;

namespace GameJam.Events.EnemyActions
{
    public class LaserBeamFireStart : IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public LaserBeamFireStart(Entity entity)
        {
            Entity = entity;
        }
    }
}
