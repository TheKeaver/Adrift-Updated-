using Audrey;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class MovementComponent : IComponent
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
    }
}
