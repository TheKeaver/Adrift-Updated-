using Audrey;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Components
{
    public class MovementComponent : IComponent, ICopyComponent
    {
        public Vector2 MovementVector;
        public bool UpdateRotationWithDirection = true;

        public MovementComponent():this(Vector2.Zero, 0.0f)
        {
        }
        public MovementComponent(Vector2 direction, float speed)
        {
            MovementVector = direction * speed;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new MovementComponent(new Vector2(MovementVector.X, MovementVector.Y), 1);
        }
    }
}
