using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace Yg.UI
{
    public class BaseTooltipUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] protected TextMeshProUGUI _tooltipTitleText;
        [SerializeField] protected TextMeshProUGUI _tooltipDescriptionText;
        [SerializeField] protected CanvasGroup _windowCanvasGroup;
        [SerializeField] protected Image _iconImage;

        [CustomHeader("DOTween Settings")]
        [SerializeField] private float _fadeDuration = .15f;

        private Tween _currentTween;

        private void OnDestroy()
        {
            _currentTween?.Kill();
        }

        public void SetData(string title, string description, Sprite icon)
        {
            _tooltipTitleText.text = title;
            _tooltipDescriptionText.text = description;
            _iconImage.sprite = icon;
        }

        public void Show()
        {
            PlayShowAnimation();
        }

        public virtual void Hide()
        {
            PlayHideAnimation();
        }

        protected void PlayShowAnimation()
        {
            if (_currentTween != null) _currentTween.Kill();

            _currentTween = _windowCanvasGroup
                .DOFade(1f, _fadeDuration)
                .OnComplete(() => _currentTween = null);
        }

        protected void PlayHideAnimation()
        {
            if (_currentTween != null) _currentTween.Kill();

            _currentTween = _windowCanvasGroup
                .DOFade(0f, _fadeDuration)
                .OnComplete(() =>
                {
                    _currentTween = null;
                    gameObject.SetActive(false);
                });
        }
    }
}
