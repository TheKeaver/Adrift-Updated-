using System;
using GameJam.UI;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Animations
{
    public class WidgetClassFadeAnimation : AnimationProcess
    {
        public Root Root
        {
            get;
            private set;
        }

        public string ClassName
        {
            get;
            private set;
        }

        public float StartAlpha
        {
            get;
            private set;
        }
        public float EndAlpha
        {
            get;
            private set;
        }

        public Easings.Functions EasingFunction
        {
            get;
            private set;
        }

        public WidgetClassFadeAnimation(Root root, string className, float startAlpha, float endAlpha, float duration, Easings.Functions easingFunction = Easings.Functions.Linear) : base(duration)
        {
            Root = root;
            ClassName = className;

            StartAlpha = startAlpha;
            EndAlpha = endAlpha;

            EasingFunction = easingFunction;
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdateAnimation()
        {
            float val = MathHelper.Lerp(StartAlpha, EndAlpha, Easings.Interpolate(ClampedAlpha, EasingFunction));
            Root.FindWidgetsByClass(ClassName).ForEach((Widget widget) =>
            {
                widget.Alpha = val;
            });
        }
    }
}
