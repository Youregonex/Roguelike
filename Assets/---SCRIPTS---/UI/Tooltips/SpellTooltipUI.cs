using TMPro;
using UnityEngine;
using Yg.Battle;

namespace Yg.UI
{
    public class SpellTooltipUI : BaseTooltipUI
    {
        [CustomHeader("Spell Tooltip Settings")]
        [SerializeField] protected TextMeshProUGUI _spellCooldownText;

        public void SetData(SpellSO spellSO)
        {
            _tooltipTitleText.text = spellSO.Name;
            _tooltipDescriptionText.text = spellSO.Description;
            _iconImage.sprite = spellSO.Icon;
            _spellCooldownText.text = spellSO.Cooldown.ToString() + "s";
        }
    }
}
