namespace GameJam
{
    /// <summary>
    /// A process that waits a duration of time before ending.
    /// </summary>
    public class WaitProcess :  Process
    {
        public float Duration { get; }
        public float Time { get; private set; }

        public WaitProcess(float duration)
        {
            Duration = duration;
            Time = duration;
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
            Time -= dt;

            if (Time <= 0)
            {
                Kill();
            }
        }
    }
}
