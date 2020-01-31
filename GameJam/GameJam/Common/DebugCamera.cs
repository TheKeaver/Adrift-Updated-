using Events;
using GameJam.Events.InputHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam.Common
{
    public class DebugCamera : Camera
    {
        private bool _dragging = false;
        private Vector2 _dragStartPosition = Vector2.Zero;
        private Vector2 _dragStartMousePosition = Vector2.Zero;

        public DebugCamera(float width, float height) : base(width, height)
        {
            UpdateFromCVars();
        }

        public override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<MouseButtonEvent>(this);
            EventManager.Instance.RegisterListener<MouseMoveEvent>(this);
            EventManager.Instance.RegisterListener<MouseScrollEvent>(this);

            base.RegisterEvents();
        }

        public override void UnregisterEvents()
        {
            base.UnregisterEvents();
        }

        public override bool Handle(IEvent evt)
        {
            if (CVars.Get<bool>("debug_force_debug_camera"))
            {
                MouseButtonEvent mouseButtonEvent = evt as MouseButtonEvent;
                if (mouseButtonEvent != null)
                {
                    if (mouseButtonEvent.LeftButtonState == ButtonState.Pressed)
                    {
                        _dragging = true;
                        _dragStartMousePosition = mouseButtonEvent.CurrentPosition;
                        _dragStartPosition = Position;
                    }
                    else
                    {
                        _dragging = false;
                    }
                    return true;
                }
                MouseMoveEvent mouseMoveEvent = evt as MouseMoveEvent;
                if (mouseMoveEvent != null)
                {
                    if (_dragging)
                    {
                        Vector2 pos = _dragStartPosition + (mouseMoveEvent.CurrentPosition - _dragStartMousePosition) * new Vector2(-1, 1);
                        CVars.Get<float>("debug_debug_camera_position_x") = pos.X;
                        CVars.Get<float>("debug_debug_camera_position_y") = pos.Y;
                        //CVars.Get<float>("debug_debug_camera_position_x") = mouseMoveEvent.CurrentPosition.X - _dragStartMousePosition.X;
                        //CVars.Get<float>("debug_debug_camera_position_y") = mouseMoveEvent.CurrentPosition.Y - _dragStartMousePosition.Y;
                        UpdateFromCVars();
                        return true;
                    }
                }
                MouseScrollEvent mouseScrollEvent = evt as MouseScrollEvent;
                if (mouseScrollEvent != null)
                {
                    CVars.Get<float>("debug_debug_camera_zoom") += mouseScrollEvent.Delta * CVars.Get<float>("debug_debug_camera_zoom_speed");
                    CVars.Get<float>("debug_debug_camera_zoom") = MathHelper.Max(0, CVars.Get<float>("debug_debug_camera_zoom"));
                    UpdateFromCVars();
                    return true;
                }
            }

            return base.Handle(evt);
        }

        private void UpdateFromCVars()
        {
            Zoom = 1 / CVars.Get<float>("debug_debug_camera_zoom");
            Position = new Vector2(CVars.Get<float>("debug_debug_camera_position_x"),
                CVars.Get<float>("debug_debug_camera_position_y"));
        }
    }
}
