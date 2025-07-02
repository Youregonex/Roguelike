using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Yg.UI
{
    public class SquadDamageUI : MonoBehaviour
    {
        [CustomHeader("Settinga")]
        [SerializeField] private Image _squadImage;
        [SerializeField] private Image _imageSlider;
        [SerializeField] private TextMeshProUGUI _squadDamageText;
        [SerializeField] private TextMeshProUGUI _damagePercentText;

        private float _squadTotalDamage;

        public void Initialize(Sprite squadSprite)
        {
            _squadImage.sprite = squadSprite;
            _imageSlider.fillAmount = 0f;
            _squadDamageText.text = "0";
            _squadTotalDamage = 0f;
            _damagePercentText.text = "(0%)";
        }

        public void AddDamage(float damage, float totalDamage)
        {
            _squadTotalDamage += damage;
            _squadDamageText.text = _squadTotalDamage.ToString("F2");

            UpdateFillAmount(totalDamage);
        }

        public void UpdateFillAmount(float totalDamage)
        {
            float t = _squadTotalDamage / totalDamage;
            _imageSlider.fillAmount = t;
            _damagePercentText.text = $"({t * 100:F1}%)";
        }
    }
}
