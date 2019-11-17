using System;
using System.Threading.Tasks;

namespace GameJam.Processes
{
    public abstract class ParallelProcess : Process
    {
        private Task _task;

        private float _dt;

        public bool RequestWorkInBackground
        {
            get;
            set;
        } = false;

        protected override void OnUpdate(float dt)
        {
            _dt = dt;
            if (CVars.Get<bool>("process_multithreading"))
            {
                if(_task != null && !_task.IsCompleted && !RequestWorkInBackground)
                {
                    throw new Exception("Parallel process did not finish from previous tick (process already running).");
                }
                _task = Task.Run(Run);
            } else
            {
                Run();
            }
        }

        private void Run()
        {
            OnRun(_dt);
        }
        protected abstract void OnRun(float dt);

        public void Await()
        {
            _task.Wait();
        }
    }
}
