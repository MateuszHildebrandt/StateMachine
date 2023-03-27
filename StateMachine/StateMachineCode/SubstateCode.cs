using System;
using UnityEngine;

namespace StateMachine
{
    public class SubstateCode
    {
        private bool _isActive;
        private bool _isDebug = false;

        public SubstateCode Parent { get; private set; }
        public string Name { get; private set; }

        public Action onEnter;
        public Action onExit;

        private StateMachineCode _stateMachine;

        public SubstateCode(StateMachineCode stateMachine, string name = null, SubstateCode parent = null)
        {
            _stateMachine = stateMachine;
            Name = name;
            SetParent(parent);
        }

        public void Enter()
        {
            if (this != null)
                _stateMachine.EnterWithParents(this);
        }

        internal void OnEnter()
        {
            if (_isActive)
                return;
            _isActive = true;

            PrintStateName("Enter");
            onEnter?.Invoke();
        }

        internal void OnExit()
        {
            if (_isActive == false)
                return;
            _isActive = false;

            PrintStateName("Exit");
            onExit?.Invoke();
        }

        internal SubstateCode On(Action onEnter)
        {
            this.onEnter += onEnter;
            return this;
        }

        internal SubstateCode Off(Action onExit)
        {
            this.onExit += onExit;
            return this;
        }

        private void SetParent(SubstateCode parent)
        {
            if (parent == null)
                return;

            Parent = parent;
        }

        private void PrintStateName(string tag)
        {
            if (_isDebug)
                Debug.Log($"{tag} state: {Name}");
        }
    }
}
