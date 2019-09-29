using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class ColoredExplosionComponent : IComponent
    {
        public Color Color
        {
            get;
            set;
        }

        public ColoredExplosionComponent(Color color)
        {
            Color = color;
        }
    }
}
