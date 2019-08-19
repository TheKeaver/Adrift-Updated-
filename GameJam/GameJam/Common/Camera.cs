using Events;
using Microsoft.Xna.Framework;
using GameJam.Events;

namespace GameJam.Common
{
    /// <summary>
    /// Wrapper around a transformation matrix to simulate a 2D camera.
    /// </summary>
    public class Camera : IEventListener
    {
        public float Zoom = 1;
        public Vector2 Position;
        public float Rotation;

        float _compensationZoom = 1;
        Rectangle _bounds;

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateScale(Zoom * _compensationZoom)
                             * Matrix.CreateRotationZ(Rotation)
                             * Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f,
                                                                    _bounds.Height * 0.5f,
                                                                   0))
                             * Matrix.CreateTranslation(new Vector3(Position.X,
                                                            Position.Y * -1,
                                                            0));
            }
        }

        public Camera(float width, float height)
        {
            HandleResize((int)width, (int)height);
        }

        public void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }
        public void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener<ResizeEvent>(this);
        }

        public Vector2 ScreenToWorldCoords(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(TransformMatrix));
        }

        public Vector2 WorldToScreenCoords(Vector2 position)
        {
            return Vector2.Transform(position, TransformMatrix);
        }

        public bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvt = evt as ResizeEvent;

            if (resizeEvt != null)
            {
                HandleResize(resizeEvt.Width, resizeEvt.Height);
            }

            return false;
        }

        void HandleResize(int w, int h)
        {
            _bounds.Width = w;
            _bounds.Height = h;

            float newAspectRatio = (float)_bounds.Width / _bounds.Height;
            if (newAspectRatio <= CVars.Get<float>("window_initial_aspect_ratio")) // Width is dominant
            {
                _compensationZoom = (float)_bounds.Width / CVars.Get<int>("window_width");
            }
            if (newAspectRatio > CVars.Get<float>("window_initial_aspect_ratio")) // Height is dominant
            {
                _compensationZoom = (float)_bounds.Height / CVars.Get<int>("window_height");
            }
        }
    }
}
