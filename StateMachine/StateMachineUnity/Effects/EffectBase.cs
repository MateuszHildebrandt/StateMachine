using UnityEngine;

namespace StateMachine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class EffectBase : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] bool hideOnAwake = true;
        [SerializeField] bool centerOnAwake = true;
        [SerializeField] protected bool useTimeScale = true;
        [Space]
        [SerializeField] protected float delay = 0;
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected AnimationCurve curve; //TODO

        internal System.Action onEnterEffect;
        internal System.Action onExitEffect;

        protected bool isActive;
        protected RectTransform rectTransform;
        protected CanvasGroup canvasGroup;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (hideOnAwake)
                SetCanvasGroup(false);

            if (centerOnAwake)
                rectTransform.anchoredPosition = Vector3.zero;
        }

        private void OnDisable()
        {
            SetCanvasGroup(false);
        }

        protected void SetCanvasGroup(bool value)
        {
            canvasGroup.alpha = (value) ? 1 : 0;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }
}
