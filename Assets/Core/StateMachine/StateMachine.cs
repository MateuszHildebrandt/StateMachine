using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace State
{
    public class StateMachine : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Substate defaultState;

        [Header("Info")]
        [ReadOnly, SerializeField] Substate _currState;
        [ReadOnly, SerializeField] Substate _lastState;

        Stack<Substate> _enterStates = new Stack<Substate>();
        Queue<Substate> _exitStates = new Queue<Substate>();

        private void Awake()
        {
            _currState = null;
            _lastState = null;
        }

        private void Start()
        {
            if (defaultState != null)
                defaultState.Enter();
        }

        private void EnterState(Substate newState)
        {
            if (newState == null)
                return;

            if (newState == _currState)
                return;

            _currState = newState;
            _currState.OnEnter();
        }

        private void ExitState(Substate exitState)
        {
            if (exitState == null)
                return;

            if (_enterStates?.Contains(exitState) == false)
                exitState?.OnExit();
        }

        internal void EnterWithParents(Substate newState)
        {
            if (newState == null)
                return;

            if (newState == _currState)
                return;

            _exitStates = new Queue<Substate>(_enterStates.Reverse());
            _enterStates.Clear();
            _enterStates.Push(newState);

            Substate state = newState;
            while (true)
            {
                if (state.transform.parent.TryGetComponent(out state))
                    _enterStates.Push(state);
                else
                    break;
            }

            _lastState = _currState;
            foreach (Substate item in _exitStates)
                ExitState(item);

            foreach (Substate item in _enterStates)
                EnterState(item);
        }

        internal void EnterLast()
        {
            if (_lastState != null)
                EnterWithParents(_lastState);
        }
    }
}
