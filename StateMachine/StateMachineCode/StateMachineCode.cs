using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public class StateMachineCode
    {
        private SubstateCode _currState;
        private SubstateCode _lastState;

        Stack<SubstateCode> _enterStates = new Stack<SubstateCode>();
        Queue<SubstateCode> _exitStates = new Queue<SubstateCode>();

        private void EnterState(SubstateCode newState)
        {
            if (newState == null)
                return;

            if (newState == _currState)
                return;

            ExitState(_currState);

            _currState = newState;
            _currState.OnEnter();
        }

        private void ExitState(SubstateCode exitState)
        {
            if (exitState == null)
                return;

            if (_enterStates?.Contains(exitState) == false)
                exitState?.OnExit();
        }

        public void EnterWithParents(SubstateCode newState)
        {
            if (newState == null)
                return;

            if (newState == _currState)
                return;

            _exitStates = new Queue<SubstateCode>(_enterStates.Reverse());
            _enterStates.Clear();
            _enterStates.Push(newState);

            SubstateCode state = newState;
            while (true)
            {
                if (state.Parent != null)
                {
                    _enterStates.Push(state.Parent);
                    state = state.Parent;
                }
                else
                    break;
            }

            _lastState = _currState;
            foreach (SubstateCode item in _exitStates)
                ExitState(item);

            foreach (SubstateCode item in _enterStates)
                EnterState(item);
        }

        public SubstateCode Add(string name = null, SubstateCode parent = null)
        {
            return new SubstateCode(this, name, parent);
        }

        public void EnterLast()
        {
            if (_lastState != null)
                EnterWithParents(_lastState);
        }
    }
}
