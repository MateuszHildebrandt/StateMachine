using StateMachine.Tools;
using System.Collections;
using UnityEngine;

namespace StateMachine
{
    public class EffectSwitch : EffectBase, IStateEnter, IStateExit
    {
        public void Switch(bool value)
        {
            if (isActive == value)
                return;

            StopAllCoroutines();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = true;
            StartCoroutine(SwitchCoroutine(value));
        }

        [ShowMethod]
        public void Toggle() => Switch(!isActive);

        private IEnumerator SwitchCoroutine(bool value)
        {
            if (delay > 0)
            {
                if (useTimeScale)
                    yield return new WaitForSeconds(delay);
                else
                    yield return new WaitForSecondsRealtime(delay);
            }     

            isActive = value;
            SetCanvasGroup(value);

            if (value)
                onEnterEffect?.Invoke();
            else
                onExitEffect?.Invoke();
        }

        void IStateEnter.OnEnter() => Switch(true);
        void IStateExit.OnExit() => Switch(false);
    }
}
