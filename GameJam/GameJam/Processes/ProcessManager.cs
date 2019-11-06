using System.Collections.Generic;
using GameJam.Processes;

namespace GameJam
{
    /// <summary>
    /// A manager for controlling the lifecycle and sequence of processes.
    /// </summary>
    public class ProcessManager
    {
        private List<Process> _processList = new List<Process>();
        private List<ParallelProcess> _parallelProcessList = new List<ParallelProcess>();

        public  Process[] Processes
        {
            get
            {
                return _processList.ToArray();
            }
        }

        public Process Attach(Process process)
        {
            // Instant processes are special; they can be ran in 0 ticks
            if (process is InstantProcess)
            {
                // Doesn't use time, doesn't matter what we enter
                process.Update(0);

                // Attach the next process, if there is one
                if (process.Next != null)
                {
                    Attach(process.Next);
                    process.SetNext(null);
                }

                return process;
            }

            _processList.Add(process);
            process.IsActive = true;

            if(process is ParallelProcess)
            {
                _parallelProcessList.Add((ParallelProcess)process);
            }

            return process;
        }

        void Detatch(Process process)
        {
            _processList.Remove(process);
            if (process is ParallelProcess)
            {
                _parallelProcessList.Remove((ParallelProcess)process);
            }
        }

        public bool HasProcesses()
        {
            return _processList.Count > 0;
        }

        public void Update(float dt, bool waitForParallelProcesses = true)
        {
            for(int i = 0; i < _parallelProcessList.Count; i++)
            {
                Process curr = _parallelProcessList[i];
                if (curr.IsActive && !curr.IsPaused)
                {
                    curr.Update(dt);
                }
            }

            for (int i = 0; i < _processList.Count; i++)
            {
                Process curr = _processList[i];

                if (!curr.IsAlive)
                {
                    if (curr.Next != null)
                    {
                        Attach(curr.Next);
                        curr.SetNext(null);
                    }
                    Detatch(curr);
                    i--;
                    continue;
                }

                if (curr is ParallelProcess)
                {
                    continue;
                }
                if (curr.IsActive && !curr.IsPaused)
                {
                    curr.Update(dt);
                }
            }

            if (waitForParallelProcesses)
            {
                WaitForParallelProcesses();
            }
        }

        public void WaitForParallelProcesses()
        {
            for (int i = 0; i < _parallelProcessList.Count; i++)
            {
                ParallelProcess parallelProcess = _parallelProcessList[i];
                if (parallelProcess != null)
                {
                    parallelProcess.Await();
                }
            }
        }

        public void KillAll()
        {
            for(int i = 0; i < _processList.Count; i++)
            {
                _processList[i].Kill();
            }
        }

        public void TogglePauseAll()
        {
            for (int i = 0; i < _processList.Count; i++)
            {
                _processList[i].TogglePause();
            }
        }
    }
}
