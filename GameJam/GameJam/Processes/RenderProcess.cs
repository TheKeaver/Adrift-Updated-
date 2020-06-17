using GameJam.Common;
using System;

namespace GameJam.Processes
{
    public abstract class RenderProcess : Process
    {
        public float Acculmulator
        {
            get;
            private set;
        }

        public float FixedDeltaTime
        {
            get
            {
                return 1 / CVars.Get<float>("tick_frequency");
            }
        }

        public void Render(float dt)
        {
            float betweenFrameAlpha = Acculmulator / FixedDeltaTime;
            OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnUpdate(float dt)
        {
            Acculmulator += dt;
            while (Acculmulator >= FixedDeltaTime)
            {
                Acculmulator -= FixedDeltaTime;
                OnFixedUpdate(FixedDeltaTime);
            }
        }

        protected abstract void OnFixedUpdate(float dt);

        protected abstract void OnRender(float dt, float betweenFrameAlpha);

        protected override void OnTogglePause()
        {
        }
    }
}
