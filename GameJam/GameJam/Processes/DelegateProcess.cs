using System;

namespace GameJam
{
    /// <summary>
    /// An instant process that can execute a delegate function.
    /// </summary>
    public class DelegateCommand : InstantProcess
    {
        readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        protected override void OnTrigger()
        {
            _action.Invoke();
        }
    }
}
