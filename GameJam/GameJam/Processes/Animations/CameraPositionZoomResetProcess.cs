using GameJam.Common;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Animations
{
    public class CameraPositionZoomResetProcess : AnimationProcess
    {
        public Camera Camera
        {
            get;
            private set;
        }

        private Vector2 _initialPosition;
        private float _initialZoom;

        private Vector2 _endPosition;
        private float _endZoom;

        public Easings.Functions EasingFunction
        {
            get;
            private set;
        }

        public CameraPositionZoomResetProcess(Camera camera, float duration, Vector2 position, float zoom, Easings.Functions easingFunction = Easings.Functions.Linear) : base(duration)
        {
            _endPosition = position;
            _endZoom = zoom;
            Camera = camera;
            EasingFunction = easingFunction;
        }

        protected override void OnInitialize()
        {
            _initialPosition = Camera.Position;
            _initialZoom = Camera.Zoom;
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdateAnimation()
        {
            Camera.Position = Vector2.Lerp(_initialPosition, _endPosition, Easings.Interpolate(ClampedAlpha, EasingFunction));
            Camera.Zoom = MathHelper.Lerp(_initialZoom, _endZoom, Easings.Interpolate(ClampedAlpha, EasingFunction));
        }
    }
}
