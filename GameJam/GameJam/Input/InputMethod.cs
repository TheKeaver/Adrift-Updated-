namespace GameJam.Input
{
    /// <summary>
    /// Abstract interface for input devices (keyboards, controllers, etc.).
    /// </summary>
    public abstract class InputMethod
    {
        protected InputSnapshot _snapshot = new InputSnapshot();

        public bool JoinKeyPressed
        {
            get;
            protected set;
        }
        public bool LeaveKeyPressed
        {
            get;
            protected set;
        }
        public bool StartKeyPressed
        {
            get;
            protected set;
        }
        public bool PauseKeyPressed
        {
            get;
            protected set;
        }

        public abstract void Update(float dt);

        public InputSnapshot GetSnapshot()
        {
            return _snapshot;
        }

    }
}
