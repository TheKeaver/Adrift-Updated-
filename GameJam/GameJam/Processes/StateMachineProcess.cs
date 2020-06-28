using System;
using System.Collections.Generic;

namespace GameJam.Processes
{
    public abstract class StateMachineProcess<T> : Process where T : struct, IConvertible, IComparable, IFormattable
    {
        public T InitialState
        {
            get;
            private set;
        }

        public T CurrentState
        {
            get;
            private set;
        }

        private Dictionary<T, State> _states;
        private Condition[] _conditions;

        public StateMachineProcess(T initialState)
        {
            _states = new Dictionary<T, State>();
            _conditions = new Condition[0];

            InitialState = initialState;
        }

        protected void OnStateExit(T currentState, T nextState) { }
        protected void OnStateEnter(T previousState, T currentState) { }

        public void ChangeState(T toState)
        {
            if(CurrentState.Equals(toState))
            {
                return;
            }

            _states[CurrentState].OnExit(toState);
            OnStateExit(CurrentState, toState);
            T previousState = toState;
            CurrentState = toState;
            _conditions = _states[CurrentState].CreateConditions(); 
            _states[CurrentState].OnEnter(previousState);
            OnStateEnter(previousState, CurrentState);
        }

        private void UpdateConditions(float dt)
        {
            foreach (Condition condition in _conditions)
            {
                condition.Update(dt);
                if(condition.HasConditionMet())
                {
                    ChangeState(condition.GetStateToSwitchTo());
                }
            }
        }

        protected override void OnUpdate(float dt)
        {
            if(_states.ContainsKey(CurrentState))
            {
                _states[CurrentState].Update(dt);
            }

            UpdateConditions(dt);
        }

        protected override void OnInitialize()
        {
            CurrentState = InitialState;
            _conditions = _states[CurrentState].CreateConditions();
            OnStateEnter(CurrentState, CurrentState);
            _states[CurrentState].OnEnter(CurrentState);
        }

        public void AddState(T state, State stateObj)
        {
            _states.Add(state, stateObj);
        }

        public abstract class Condition
        {
            public abstract void Update(float dt);
            public abstract bool HasConditionMet();

            public abstract T GetStateToSwitchTo();
        }

        public abstract class State
        {
            public abstract Condition[] CreateConditions();

            public abstract void OnEnter(T previousState);
            public abstract void Update(float dt);
            public abstract void OnExit(T nextState);
        }
    }
}
