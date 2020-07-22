using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    /// <summary>
    /// A component for holding a position vector and a rotation.
    /// </summary>
    public class TransformComponent : IComponent, ICopyComponent
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
            _position.X += x;
            _position.Y += y;
        }
        public void SetPosition(Vector2 position, bool reset = false)
        {
            SetPosition(position.X, position.Y, reset);
        }

        public void SetPosition(float x, float y, bool reset = false)
        {
            _position.X = x;
            _position.Y = y;

            if(reset)
            {
                _lastPosition.X = Position.X;
                _lastPosition.Y = Position.Y;
            }
        }

        public void Rotate(float delta)
        {
            Rotation += delta;
        }
        public void SetRotation(float rotation, bool reset = false)
        {
            Rotation = rotation;
            if (reset)
            {
                LastRotation = Rotation;
            }
        }

        public void SetScale(float scale, bool reset = false)
        {
            Scale = scale;
            if(reset)
            {
                LastScale = Scale;
            }
        }

        public void Interpolate(float alpha, out Vector2 position, out float rotation, out float scale)
        {
            position = Vector2.Lerp(LastPosition, Position, alpha);
            rotation = MathUtils.LerpAngle(LastRotation, Rotation, alpha);
            scale = MathHelper.Lerp(LastScale, Scale, alpha);
        }

        public void ResetAll()
        {
            LastPosition = Position;
            LastRotation = Rotation;
            LastScale = Scale;
        }

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new TransformComponent(new Vector2(Position.X, Position.Y), Rotation);
        }
    }
}
