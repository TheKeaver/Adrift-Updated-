using System;
using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    /// <summary>
    /// A component for holding a position vector and a rotation.
    /// </summary>
    public class TransformComponent : IComponent
    {
        public TransformComponent() : this(Vector2.Zero)
        {
        }

        public TransformComponent(Vector2 position) : this(position, 0)
        {
        }

        public TransformComponent(Vector2 position, float rotation)
        {
            Position = position;
            Rotation = rotation;

            LastPosition = position;
            LastRotation = rotation;
        }

        Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            private set
            {
                _position = value;
            }
        }
        float _rotation;
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            private set
            {
                _rotation = value;
            }
        }

        Vector2 _lastPosition;
        public Vector2 LastPosition
        {
            get
            {
                return _lastPosition;
            }
            private set
            {
                _lastPosition = value;
            }
        }
        float _lastRotation;
        public float LastRotation
        {
            get
            {
                return _lastRotation;
            }
            private set
            {
                _lastRotation = value;
            }
        }

        public void Move(Vector2 delta)
        {
            Move(delta.X, delta.Y);
        }
        public void Move(float x, float y)
        {
            _lastPosition.X = _position.X;
            _lastPosition.Y = _position.Y;

            _position.X += x;
            _position.Y += y;
        }
        public void SetPosition(Vector2 position)
        {
            SetPosition(position.X, position.Y);
        }

        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
            _lastPosition.X = x;
            _lastPosition.Y = y;
        }

        public void Rotate(float delta)
        {
            _lastRotation = _rotation;
            _rotation += delta;
        }
        public void SetRotation(float rotation)
        {
            _rotation = rotation;
            _lastRotation = rotation;
        }
    }
}
