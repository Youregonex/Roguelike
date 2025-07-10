using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yg.GameData;

namespace Yg.UI
{
    public class StatModifierUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _statIconImage;
        [SerializeField] private TextMeshProUGUI _statText;

        public void SetData(Sprite icon, StatModifier statModifier)
        {
            _statIconImage.sprite = icon;
            _statText.text = $"+{(statModifier.Value * 100):F1}% {statModifier.StatType}";
            _statText.color = Color.green;
        }
    }
}
