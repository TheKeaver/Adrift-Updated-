using Audrey;
using System;

namespace GameJam.Components
{
    public class RotationComponent : IComponent, ICopyComponent
    {
        public float RotationSpeed;

        public RotationComponent(float rotationSpeed)
        {
            RotationSpeed = rotationSpeed;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new RotationComponent(RotationSpeed);
        }
    }
}
