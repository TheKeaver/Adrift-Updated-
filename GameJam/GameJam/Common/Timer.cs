namespace GameJam
{
    /// <summary>
    /// Implementation of a countdown timer.
    /// </summary>
    public class Timer
    {
        float _duration;
        float _elapsedTime;

        public float Elapsed
        {
            get
            {
                return _elapsedTime;
            }
        }

        public Timer(float duration)
        {
            _duration = duration;
        }

        public void Update(float dt)
        {
            _elapsedTime += dt;
        }

        public bool HasElapsed()
        {
            return _elapsedTime >= _duration;
        }

        public void Reset()
        {
            _elapsedTime = 0;
        }

        public void Reset(float newDuration)
        {
            _duration = newDuration;
            Reset();
        }
    }
}
