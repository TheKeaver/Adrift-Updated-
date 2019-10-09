using System;
using Audrey;
using Events;

namespace GameJam.Events.EnemyActions
{
    public class LaserBeamWarmUpStart : IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public LaserBeamWarmUpStart(Entity entity)
        {
            Entity = entity;
        }
    }
}
