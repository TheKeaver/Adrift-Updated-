using Audrey;

namespace GameJam.Components
{
    public class LaserBeamComponent : IComponent
    {
        public Entity ReflectionBeamEntity = null;
        public bool ComputeReflection;
        public bool InteractWithShield;
        public float Thickness = 4; // TODO: CVar

        public LaserBeamComponent(bool computeReflection = true,
            bool interactWithShield = true)
        {
            ComputeReflection = true;
            InteractWithShield = interactWithShield;
        }
    }
}
