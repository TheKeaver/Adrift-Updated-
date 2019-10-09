using Audrey;

namespace GameJam.Components
{
    public class ProjectileComponent : IComponent
    {
        public int BouncesLeft;
        
        public ProjectileComponent(int totalBounces)
        {
            BouncesLeft = totalBounces;
        }
    }
}
