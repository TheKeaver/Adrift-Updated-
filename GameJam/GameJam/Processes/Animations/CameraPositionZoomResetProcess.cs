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

        public Easings.Functions EasingFunction
        {
            get;
            private set;
        }

        public CameraPositionZoomResetProcess(Camera camera, float duration, Easings.Functions easingFunction = Easings.Functions.Linear) : base(duration)
        {
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
            Camera.Position = Vector2.Lerp(_initialPosition, Vector2.Zero, Easings.Interpolate(ClampedAlpha, EasingFunction));
            Camera.Zoom = MathHelper.Lerp(_initialZoom, 1, Easings.Interpolate(ClampedAlpha, EasingFunction));
        }
    }
}
