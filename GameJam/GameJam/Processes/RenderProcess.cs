namespace GameJam.Processes
{
    public abstract class RenderProcess : Process
    {
        public float Acculmulator
        {
            get;
            private set;
        }

        public void Render(float dt)
        {
            float betweenFrameAlpha = Acculmulator / (1 / CVars.Get<float>("tick_frequency"));
            OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnUpdate(float dt)
        {
            Acculmulator += dt;
            while (Acculmulator >= 1 / CVars.Get<float>("tick_frequency"))
            {
                float fixedDt = 1 / CVars.Get<float>("tick_frequency");
                Acculmulator -= fixedDt;
                OnFixedUpdate(fixedDt);
            }
        }

        protected abstract void OnFixedUpdate(float dt);

        protected abstract void OnRender(float dt, float betweenFrameAlpha);
    }
}
