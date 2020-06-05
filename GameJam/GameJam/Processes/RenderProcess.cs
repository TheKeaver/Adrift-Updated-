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

        public float ElapsedUpdateTime
        {
            get;
            private set;
        }
        public float ElapsedRenderTime
        {
            get;
            private set;
        }

        public readonly float FixedDeltaTime = 1 / CVars.Get<float>("tick_frequency");

        public void Render(float dt)
        {
            ElapsedRenderTime += dt;

            float betweenFrameAlpha;
            if(IsPaused)
            {
                betweenFrameAlpha = 0;
            } else
            {
                betweenFrameAlpha = MathUtils.InverseLerp(ElapsedUpdateTime, ElapsedUpdateTime + FixedDeltaTime,
                    ElapsedRenderTime);
            }
            OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnUpdate(float dt)
        {
            Acculmulator += dt;
            while (Acculmulator >= FixedDeltaTime)
            {
                Acculmulator -= FixedDeltaTime;
                OnFixedUpdate(FixedDeltaTime);

                ElapsedUpdateTime += FixedDeltaTime;
            }
        }

        protected abstract void OnFixedUpdate(float dt);

        protected abstract void OnRender(float dt, float betweenFrameAlpha);

        protected override void OnTogglePause()
        {
            if(!IsPaused)
            {
                // Reset ElapsedUpdateTime and ElapsedRenderTime, otherwise they'll be
                // out of sync and will cause some weird extrapolation issues.
                ElapsedUpdateTime = 0;
                ElapsedRenderTime = 0;
            }
        }
    }
}
