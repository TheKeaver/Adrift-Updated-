using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class LaserBeamComponent : IComponent
    {
        public Entity ReflectionBeamEntity = null;
        public bool ComputeReflection;
        public bool InteractWithShield;
        public float Thickness = CVars.Get<float>("laser_enemy_fire_thickness");
        public Color Color;

        public LaserBeamComponent(bool computeReflection = true,
            bool interactWithShield = true)
        {
            ComputeReflection = true;
            InteractWithShield = interactWithShield;
            Color = Color.Red;
        }
    }
}
