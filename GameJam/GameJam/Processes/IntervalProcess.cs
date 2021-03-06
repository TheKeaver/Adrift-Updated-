﻿namespace GameJam
{
    /// <summary>
    /// An abstract process that ticks in specified time intervals.
    /// </summary>
    public abstract class IntervalProcess :  Process
    {
        float _acculmulator;

        public float Interval { get; set; }

        public IntervalProcess(float interval)
        {
            Interval = interval;
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
            _acculmulator += dt;
            while (_acculmulator >= Interval)
            {
                OnTick(Interval);
                _acculmulator -= Interval;
            }
        }

        protected abstract void OnTick(float interval);
    }
}
