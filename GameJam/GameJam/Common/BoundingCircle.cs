using System;
using Microsoft.Xna.Framework;

namespace GameJam.Common
{
    public class BoundingCircle
    {
        public Vector2 Position
        {
            get;
            set;
        }

        private float _radiusSquared;
        public float RadiusSquared
        {
            get
            {
                return _radiusSquared;
            }
            set
            {
                _radiusSquared = value;
                _radius = (float)Math.Sqrt(value);
            }
        }
        private float _radius;
        public float Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
                _radiusSquared = value * value;
            }
        }

        public BoundingCircle(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool Contains(Vector2 point)
        {
            return (point - Position).LengthSquared() <= RadiusSquared;
        }
        public bool Contains(BoundingCircle other)
        {
            return (other.Position - Position).LengthSquared() <= RadiusSquared + other.RadiusSquared;
        }
    }
}
