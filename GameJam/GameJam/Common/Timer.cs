namespace GameJam
{
    /// <summary>
    /// Implementation of a countdown timer.
    /// </summary>
    public class Timer
    {
        float _duration;

        public float Elapsed { get; private set; }

        public float Alpha {
            get
            {
                return Elapsed / _duration;
            }
        }

        public Timer(float duration)
        {
            _duration = duration;
        }

        public void Update(float dt)
        {
            Elapsed += dt;
        }

        public bool HasElapsed()
        {
            return Elapsed >= _duration;
        }

        public void Reset()
        {
            Elapsed = 0;
        }

        public void Reset(float newDuration)
        {
            _duration = newDuration;
            Reset();
        }
    }
}
