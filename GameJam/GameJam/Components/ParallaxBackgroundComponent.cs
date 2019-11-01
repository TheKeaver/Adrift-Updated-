using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class ParallaxBackgroundComponent : IComponent
    {
        public float Strength;
        public Vector2 Origin;

        public ParallaxBackgroundComponent(float strength)
            :this(strength, Vector2.Zero)
        {
        }
        public ParallaxBackgroundComponent(float strength, Vector2 origin)
        {
            Strength = strength;
            Origin = origin;
        }
    }
}
