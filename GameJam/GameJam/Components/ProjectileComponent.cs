using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class ProjectileComponent : IComponent
    {
        public int BouncesLeft;
        public Color Color;
        public Player LastBouncedBy = null;
        
        public ProjectileComponent(int totalBounces, Color color)
        {
            BouncesLeft = totalBounces;
            Color = color;
        }
    }
}
