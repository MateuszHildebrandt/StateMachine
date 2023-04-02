using StateMachine.Tools;
using System.Collections;
using UnityEngine;

namespace StateMachine
{
    public class EffectFade : EffectBase, IStateEnter, IStateExit
    {
        public void Fade(bool value)
        {
            if (isActive == value)
                return;

            StopAllCoroutines();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = true;
            StartCoroutine(FadeCoroutine(value));
        }

        [ShowMethod]
        public void Toggle() => Fade(!isActive);

        private IEnumerator FadeCoroutine(bool value)
        {
            if (delay > 0)
            {
                if (useTimeScale)
                    yield return new WaitForSeconds(delay);
                else
                    yield return new WaitForSecondsRealtime(delay);
            }

            float start;
            float currValue = canvasGroup.alpha;

            if (value)
                start = (currValue < 1) ? currValue : 1;
            else
                start = (1 - currValue < 1) ? 1 - currValue : 1;

            for (float i = start; i < 1; i += (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime) / duration)
            {
                canvasGroup.alpha = (value) ? i : 1 - i;
                yield return null;
            }

            isActive = value;
            SetCanvasGroup(value);

            if (value)
                onEnterEffect?.Invoke();
            else
                onExitEffect?.Invoke();
        }

        void IStateEnter.OnEnter() => Fade(true);
        void IStateExit.OnExit() => Fade(false);
    }
}
