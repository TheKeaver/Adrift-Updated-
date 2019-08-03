using Audrey;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class MovementComponent : IComponent
    {
        public Vector2 direction;

        public MovementComponent() :this(Vector2.Zero)
        {
        }
        public MovementComponent(Vector2 direction)
        {
            this.direction = direction;
        }
    }
}
