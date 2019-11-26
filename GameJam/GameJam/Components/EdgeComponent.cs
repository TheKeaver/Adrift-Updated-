using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class EdgeComponent : IComponent
    {
        public Vector2 Normal;
        public EdgeComponent(Vector2 norm)
        {
            Normal = norm;
        }
    }
}
