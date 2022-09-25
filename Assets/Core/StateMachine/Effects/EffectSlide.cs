using System.Collections;
using UnityEngine;

namespace State
{
    public class EffectSlide : EffectBase, IStateEnter, IStateExit
    {
        public enum MoveDirection { Up, Down, Left, Right, }
        public MoveDirection moveDirection;

        public void Slide(bool value)
        {
            if (isActive == value)
                return;

            StopAllCoroutines();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = true;
            StartCoroutine(SlideCoroutine(value));
        }

        [ShowMethod]
        public void Toggle() => Slide(!isActive);

        private IEnumerator SlideCoroutine(bool value)
        {
            if (delay > 0)
            {
                if (useTimeScale)
                    yield return new WaitForSeconds(delay);
                else
                    yield return new WaitForSecondsRealtime(delay);
            }

            onEnterEffect?.Invoke();

            Vector3 from, to;

            if (value)
            {
                canvasGroup.alpha = 1;
                from = SetSlideTarget();
                to = Vector3.zero;

                rectTransform.anchoredPosition = from;
            }
            else
            {
                from = Vector3.zero;
                to = SetSlideTarget();             
            }

            for (float i = 0; i < 1; i += (useTimeScale ? Time.deltaTime : Time.unscaledDeltaTime) / duration)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(from, to, i);
                yield return null;
            }

            rectTransform.anchoredPosition = to;
            if (value == false)
            {
                canvasGroup.alpha = 0;
                onExitEffect?.Invoke();
            }

            isActive = value;
            SetCanvasGroup(value);
            onExitEffect?.Invoke();
        }

        private Vector3 SetSlideTarget()
        {
            Vector3 from = Vector3.zero;

            if (moveDirection == MoveDirection.Up)
                from += Vector3.down * rectTransform.rect.height;
            else if (moveDirection == MoveDirection.Down)
                from += Vector3.up * rectTransform.rect.height;
            else if (moveDirection == MoveDirection.Left)
                from += Vector3.right * rectTransform.rect.width;
            else if (moveDirection == MoveDirection.Right)
                from += Vector3.left * rectTransform.rect.width;

            return from;
        }

        void IStateEnter.OnEnter() => Slide(true);
        void IStateExit.OnExit() => Slide(false);
    }
}
