﻿using Events;
using Microsoft.Xna.Framework;
using GameJam.Events;

namespace GameJam.Common
{
    /// <summary>
    /// Wrapper around a transformation matrix to simulate a 2D camera.
    /// </summary>
    public class Camera : IEventListener
    {
        private float _zoom = 1;
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                // Enter code that changes boundingRect
                CalculateBoundingRect();
            }
        }

        private Vector2 _position = Vector2.Zero;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set 
            {
                _position = value;
                // Enter code that changes boundingRect
                CalculateBoundingRect();
            }
        }
        public float Rotation;

        float _compensationZoom = 1;
        Rectangle _bounds;
        public BoundingRect BoundingRect
        {
            get;
            private set;
        }

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(_position.X * -1,
                        _position.Y,
                        0))
                    * Matrix.CreateRotationZ(Rotation)
                    * Matrix.CreateScale(Zoom * _compensationZoom * CVars.Get<float>("debug_gameplay_camera_zoom"))
                    * Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f,
                        _bounds.Height * 0.5f,
                        0));
            }
        }

        public Camera(float width, float height)
        {
            BoundingRect = new BoundingRect(_position.X, _position.Y, width, height);
            HandleResize((int)width, (int)height);
        }

        public virtual void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }
        public virtual void UnregisterEvents()
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

        public virtual bool Handle(IEvent evt)
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
            CalculateBoundingRect();
        }

        void CalculateBoundingRect()
        {
            BoundingRect = new BoundingRect(_position.X - (_bounds.Width / _zoom / _compensationZoom) / 2,
                    _position.Y - (_bounds.Height / _zoom / _compensationZoom) / 2,
                    _bounds.Width / _zoom / _compensationZoom,
                    _bounds.Height / _zoom / _compensationZoom);
        }
    }
}
