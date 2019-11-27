using GameJam.Common;
using Microsoft.Xna.Framework;

namespace GameJam.Processes
{
    public abstract class AnimationProcess : Process
    {
        public Timer Timer
        {
            get;
            private set;
        }

        public float Alpha
        {
            get
            {
                return Timer.Alpha;
            }
        }

        public float ClampedAlpha
        {
            get
            {
                return MathHelper.Clamp(Alpha, 0, 1);
            }
        }

        public AnimationProcess(float duration)
        {
            Timer = new Timer(duration);
        }

        protected override void OnKill()
        {
            Timer.Reset();
            Timer.Update(Timer.Duration);
            OnUpdateAnimation();
        }

        protected override void OnUpdate(float dt)
        {
            Timer.Update(dt);
            if(Timer.HasElapsed())
            {
                Kill();
                return;
            }
            OnUpdateAnimation();
        }

        protected abstract void OnUpdateAnimation();
    }
}
