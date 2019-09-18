using Audrey;
using Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Events
{
    public class CollisionEndEvent : IEvent
    {
        public Entity EntityA;
        public Entity EntityB;

        public CollisionEndEvent(Entity entityA, Entity entityB)
        {
            EntityA = entityA;
            EntityB = entityB;
        }
    }
}
