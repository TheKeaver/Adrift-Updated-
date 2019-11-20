namespace GameJam.Common
{
    /// <summary>
    /// Implementation of a countdown timer.
    /// </summary>
    public class Timer
    {
        public float Duration { get; private set; }

        public float Elapsed { get; private set; }

        public float Alpha {
            get
            {
                return Elapsed / Duration;
            }
        }

        public Timer(float duration)
        {
            Duration = duration;
        }

        public void Update(float dt)
        {
            Elapsed += dt;
        }

        public bool HasElapsed()
        {
            return Elapsed >= Duration;
        }

        public void Reset()
        {
            Elapsed = 0;
        }

        public void Reset(float newDuration)
        {
            Duration = newDuration;
            Reset();
        }
    }
}
