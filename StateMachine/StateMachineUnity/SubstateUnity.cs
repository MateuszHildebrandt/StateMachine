using System.Collections;
using UnityEngine;

namespace StateMachine
{
    public interface IStateEnter
    {
        public void OnEnter();
    }

    public interface IStateExit
    {
        public void OnExit();
    }

    public class SubstateUnity : MonoBehaviour
    {
        private bool _isActive;
        private bool _isDebug = false;

        private StateMachineUnity _myStateMachine;
        private StateMachineUnity MyStateMachine
        {
            get
            {
                if (_myStateMachine == null)
                    _myStateMachine = GetComponentInParent<StateMachineUnity>();
                return _myStateMachine;
            }
        }

        void Awake()
        {
            SetChildsActive(false);
        }

        private IStateEnter[] _statesEnter;
        private IStateEnter[] StatesEnter
        {
            get => GetIStates(ref _statesEnter);
        }

        private IStateExit[] _statesExit;
        private IStateExit[] StatesExit
        {
            get => GetIStates(ref _statesExit);
        }

        private T[] GetIStates<T>(ref T[] states)
        {
            if (states == null)
                states = GetComponents<T>();
            return states;
        }

        [ShowMethod]
        public void Enter()
        {
            if (this != null)
                MyStateMachine.EnterWithParents(this);
        }

        internal void OnEnter()
        {
            if (_isActive)
                return;
            _isActive = true;

            PrintStateName("Enter");
            SetChildsActive(true);

            StopAllCoroutines();

            if (isActiveAndEnabled)
            {
                StartCoroutine(WaitFor(0.1f, () =>
                {
                    foreach (IStateEnter item in StatesEnter)
                        item.OnEnter();
                }));
            }
        }

        internal void OnExit()
        {
            if (_isActive == false)
                return;
            _isActive = false;

            PrintStateName("Exit");
            StopAllCoroutines();

            if (isActiveAndEnabled)
            {
                StartCoroutine(WaitFor(0.1f, () =>
                {
                    foreach (IStateExit item in StatesExit)
                        item.OnExit();
                }));
            }

            SetChildsActive(false);
        }

        private void SetChildsActive(bool value)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.GetComponent<SubstateUnity>() == null)
                    child.gameObject.SetActive(value);
            }
        }

        private IEnumerator WaitFor(float seconds, System.Action onDone)
        {
            yield return new WaitForSeconds(seconds);
            onDone.Invoke();
        }

        private void PrintStateName(string tag)
        {
            if (_isDebug)
                Debug.Log($"{tag} state: {gameObject.name}", this);
        }
    }
}
