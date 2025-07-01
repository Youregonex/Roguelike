using UnityEngine;
using TMPro;

namespace Yg.UI
{
    public class TooltipUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private TextMeshProUGUI _tooltipTitleText;
        [SerializeField] private TextMeshProUGUI _tooltipDescriptionText;
        [SerializeField] private TextMeshProUGUI _spellCooldownText;

        public void SetTooltipData(string title, string description)
        {
            _tooltipTitleText.text = title;
            _tooltipDescriptionText.text = description;
            _spellCooldownText.text = "";
        }

        public void SetTooltipData(string title, string description, float cooldown)
        {
            _tooltipTitleText.text = title;
            _tooltipDescriptionText.text = description;
            _spellCooldownText.text = $"Cooldown: {cooldown}s";
        }
    }
}
