using Events;

namespace Audrey.Events
{
    public class ComponentAddedEvent : IEvent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public IComponent Component
        {
            get;
            private set;
        }

        public ComponentAddedEvent(Entity entity, IComponent component)
        {
            Entity = entity;
            Component = component;
        }
    }
}
