using Audrey;
using Events;

namespace GameJam.Events.GameLogic
{
    public class CollisionStartEvent : IEvent
    {
        public Entity EntityA;
        public Entity EntityB;

        public CollisionStartEvent(Entity entityA, Entity entityB)
        {
            EntityA = entityA;
            EntityB = entityB;
        }
    }
}
