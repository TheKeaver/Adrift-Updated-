namespace GameJam
{
    /// <summary>
    /// An abstract process that executes in one tick.
    /// </summary>
    public abstract class InstantProcess : Process
    {
        protected override void OnInitialize()
        {
            OnTrigger();
            Kill();
        }

        protected abstract void OnTrigger();

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
        }
    }
}
