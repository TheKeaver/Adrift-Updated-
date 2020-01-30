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
        public bool EnableCompensationZoom = true;
        Rectangle _bounds;

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(Position.X * -1,
                        Position.Y,
                        0))
                    * Matrix.CreateRotationZ(Rotation)
                    * Matrix.CreateScale(Zoom * (EnableCompensationZoom ? _compensationZoom : 1) * CVars.Get<float>("debug_camera_zoom"))
                    * Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f,
                        _bounds.Height * 0.5f,
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
            float desiredAspectRatio = CVars.Get<float>("screen_width") / CVars.Get<float>("screen_height");
            if (newAspectRatio <= desiredAspectRatio) // Width is dominant
            {
                _compensationZoom = (float)_bounds.Width / CVars.Get<float>("screen_width");
            }
            if (newAspectRatio > desiredAspectRatio) // Height is dominant
            {
                _compensationZoom = (float)_bounds.Height / CVars.Get<float>("screen_height");
            }
        }
    }
}
