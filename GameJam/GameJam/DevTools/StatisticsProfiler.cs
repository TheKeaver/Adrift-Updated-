using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace GameJam.DevTools
{
    public class StatisticsProfiler
    {
        public float TimeBetweenTicks
        {
            get;
            private set;
        }
        public float AverageTimeBetweenTicks
        {
            get;
            private set;
        }
        public float UpdateTime
        {
            get;
            private set;
        }
        public float AverageUpdateTime
        {
            get;
            private set;
        }

        public float TimeBetweenFrames
        {
            get;
            private set;
        }
        public float AverageTimeBetweenFrames
        {
            get;
            private set;
        }
        public float DrawTime
        {
            get;
            private set;
        }
        public float AverageDrawTime
        {
            get;
            private set;
        }

        public int Particles
        {
            get;
            private set;
        }
        public float AverageParticles
        {
            get;
            private set;
        }

        private List<float> _betweenTickTimes = new List<float>();
        private List<float> _updateTimes = new List<float>();
        private List<float> _betweenFrameTimes = new List<float>();
        private List<float> _drawTimes = new List<float>();
        private List<int> _particles = new List<int>();

        private Stopwatch _updateStopwatch = new Stopwatch();
        private Stopwatch _drawStopwatch = new Stopwatch();

        public void BeginUpdate(GameTime gameTime)
        {
            TimeBetweenTicks = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _betweenTickTimes.Add(TimeBetweenTicks);
            EnforceMaxListSize(_betweenTickTimes, CVars.Get<int>("debug_statistics_average_between_ticks_sample"));
            AverageTimeBetweenTicks = ComputeAverage(_betweenTickTimes);

            _updateStopwatch.Reset();
            _updateStopwatch.Start();
        }
        public void EndUpdate()
        {
            _updateStopwatch.Stop();
            UpdateTime = (float)_updateStopwatch.Elapsed.TotalSeconds;
            _updateTimes.Add(UpdateTime);
            EnforceMaxListSize(_updateTimes, CVars.Get<int>("debug_statistics_average_update_sample"));
            AverageUpdateTime = ComputeAverage(_updateTimes);
        }

        public void BeginDraw(GameTime gameTime)
        {
            TimeBetweenFrames = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _betweenFrameTimes.Add(TimeBetweenFrames);
            EnforceMaxListSize(_betweenFrameTimes, CVars.Get<int>("debug_statistics_average_between_frames_sample"));
            AverageTimeBetweenFrames = ComputeAverage(_betweenFrameTimes);

            _drawStopwatch.Reset();
            _drawStopwatch.Start();
        }
        public void EndDraw()
        {
            _drawStopwatch.Stop();
            DrawTime = (float)_drawStopwatch.Elapsed.TotalSeconds;
            _drawTimes.Add(DrawTime);
            EnforceMaxListSize(_drawTimes, CVars.Get<int>("debug_statistics_average_draw_sample"));
            AverageDrawTime = ComputeAverage(_drawTimes);
        }

        public void PushParticleCount(int count)
        {
            _particles.Add(count);
            EnforceMaxListSize(_particles, CVars.Get<int>("debug_statistics_average_particle_sample"));
            AverageParticles = ComputeAverage(_particles);
        }

        private void EnforceMaxListSize<T>(List<T> list, int max)
        {
            if(list.Count > max)
            {
                list.RemoveRange(0, list.Count - max);
            }
        }
        private float ComputeAverage(List<float> list)
        {
            float sum = 0;
            for(int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }
            return sum / list.Count;
        }
        private float ComputeAverage(List<int> list)
        {
            float sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i];
            }
            return sum / list.Count;
        }
    }
}
