using Events;

namespace Audrey.Events
{
    public class ComponentAddedEvent<T> : IEvent where T: IComponent
    {
        public Entity Entity
        {
            get;
            private set;
        }

        public T Component
        {
            get;
            private set;
        }

        public ComponentAddedEvent(Entity entity, T component)
        {
            Entity = entity;
            Component = component;
        }
    }
}
