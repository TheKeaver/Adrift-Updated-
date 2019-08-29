using Audrey;

namespace GameJam.Components
{
    public class RotationComponent : IComponent
    {
        public float RotationSpeed;

        public RotationComponent(float rotationSpeed)
        {
            RotationSpeed = rotationSpeed;
        }
    }
}
