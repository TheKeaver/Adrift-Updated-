using System;

namespace GameJam
{
    /// <summary>
    /// An instant process that can execute a delegate function.
    /// </summary>
    public class DelegateProcess : InstantProcess
    {
        readonly Action _action;

        public DelegateProcess(Action action)
        {
            _action = action;
        }

        protected override void OnTrigger()
        {
            _action.Invoke();
        }
    }
}
