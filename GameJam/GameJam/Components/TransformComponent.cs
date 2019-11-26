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

        public TransformComponent(Vector2 position, float rotation) : this(position, rotation, 1)
        {

        }

        public TransformComponent(Vector2 position, float rotation, float scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;

            LastPosition = position;
            LastRotation = rotation;
            LastScale = scale;
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
        public float Rotation { get; private set; }
        public float Scale { get; private set; }

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
        public float LastRotation { get; private set; }
        public float LastScale { get; private set; }

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
            LastRotation = Rotation;
            Rotation += delta;
        }
        public void SetRotation(float rotation)
        {
            Rotation = rotation;
            LastRotation = rotation;
        }

        public void ChangeScale(float scale, bool reset = false)
        {
            LastScale = Scale;
            Scale = scale;
            if(reset)
            {
                LastScale = Scale;
            }
        }
    }
}
