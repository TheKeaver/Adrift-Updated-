using Audrey;

namespace GameJam.Components
{
    public class GravityHolePingComponent : IComponent
    {
        public Entity Owner;

        public GravityHolePingComponent(Entity owner)
        {
            Owner = owner;
        }
    }
}
