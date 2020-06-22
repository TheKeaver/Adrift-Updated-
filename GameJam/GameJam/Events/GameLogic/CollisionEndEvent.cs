using Audrey;
using Events;

namespace GameJam.Events.GameLogic
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
