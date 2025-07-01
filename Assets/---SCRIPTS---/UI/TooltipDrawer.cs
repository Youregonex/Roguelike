using UnityEngine;
using Yg.Battle;
using Yg.GameData.Perks;

namespace Yg.UI
{
    public class TooltipDrawer : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private TooltipUI _perkTooltip;
        [SerializeField] private Vector2 _tooltipOffset;

        private void Awake()
        {
            _perkTooltip.gameObject.SetActive(false);
        }

        public void ShowTooltip(Perk perk, RectTransform rectTransform)
        {
            _perkTooltip.gameObject.SetActive(true);
            _perkTooltip.transform.position = (Vector2)rectTransform.position + _tooltipOffset;
            _perkTooltip.SetTooltipData(perk.Name, perk.PerkDescription);
        }

        public void ShowTooltip(SpellSO spellSO, RectTransform rectTransform)
        {
            _perkTooltip.gameObject.SetActive(true);
            _perkTooltip.transform.position = (Vector2)rectTransform.position + _tooltipOffset;
            _perkTooltip.SetTooltipData(spellSO.Name, spellSO.Description, spellSO.Cooldown);
        }

        public void HideTooltip()
        {
            _perkTooltip.gameObject.SetActive(false);
        }
    }
}
