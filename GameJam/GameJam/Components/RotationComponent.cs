using Audrey;

namespace GameJam.Components
{
    public class RotationComponent : IComponent
    {
        public float rotationSpeed;

        public RotationComponent(float rotationSpeed)
        {
            this.rotationSpeed = rotationSpeed;
        }
    }
}
