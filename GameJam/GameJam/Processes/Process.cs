namespace GameJam
{
    /// <summary>
    /// An action that takes place over multiple frames that can be
    /// chained in sequence to other processes.
    /// </summary>
    public abstract class Process
    {
        public bool IsInitialized
        {
            get;
            private set;
        } = false;
        private bool _kill = false;
        public bool IsAlive
        {
            get
            {
                return !_kill;
            }
        }
        public bool IsActive
        {
            get;
            internal set; // This is internal because ProcessManager sets the IsActive flag
        } = true;
        public bool IsPaused
        {
            get;
            private set;
        } = false;

        public Process Next
        {
            get;
            private set;
        }

        public Process SetNext(Process process)
        {
            Next = process;
            return Next;
        }

        internal void Update(float dt)
        {
            if (!IsInitialized)
            {
                OnInitialize();
                IsInitialized = true;
            }
            OnUpdate(dt);
        }
        protected abstract void OnUpdate(float dt);

        protected abstract void OnInitialize();

        public void Kill()
        {
            _kill = true;
            OnKill();
        }
        protected abstract void OnKill();

        public void TogglePause()
        {
            IsPaused = !IsPaused;
            OnTogglePause();
        }
        protected abstract void OnTogglePause();
    }
}
