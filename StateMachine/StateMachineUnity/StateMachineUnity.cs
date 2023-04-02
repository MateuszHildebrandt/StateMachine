using StateMachine.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{
    public class StateMachineUnity : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] SubstateUnity defaultState;

        [Header("Info")]
        [ReadOnly, SerializeField] SubstateUnity _currState;
        [ReadOnly, SerializeField] SubstateUnity _lastState;

        Stack<SubstateUnity> _enterStates = new Stack<SubstateUnity>();
        Queue<SubstateUnity> _exitStates = new Queue<SubstateUnity>();

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

        private void EnterState(SubstateUnity newState)
        {
            if (newState == null)
                return;

            if (newState == _currState)
                return;

            _currState = newState;
            _currState.OnEnter();
        }

        private void ExitState(SubstateUnity exitState)
        {
            if (exitState == null)
                return;

            if (_enterStates?.Contains(exitState) == false)
                exitState?.OnExit();
        }

        public void EnterWithParents(SubstateUnity newState)
        {
            if (newState == null)
                return;

            if (newState == _currState)
                return;

            _exitStates = new Queue<SubstateUnity>(_enterStates.Reverse());
            _enterStates.Clear();
            _enterStates.Push(newState);

            SubstateUnity state = newState;
            while (true)
            {
                if (state.transform.parent.TryGetComponent(out state))
                    _enterStates.Push(state);
                else
                    break;
            }

            _lastState = _currState;
            foreach (SubstateUnity item in _exitStates)
                ExitState(item);

            foreach (SubstateUnity item in _enterStates)
                EnterState(item);
        }

        [ShowMethod]
        public void EnterLast()
        {
            if (_lastState != null)
                EnterWithParents(_lastState);
        }
    }
}
