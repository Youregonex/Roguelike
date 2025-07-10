using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yg.GameData.Perks;
using Zenject;

namespace Yg.UI
{
    public class PerkUI : TooltipHolderUI
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _perkImage;

        private PerkSO _perk;

        [Inject]
        private void Construct(TooltipDrawer tooltipDrawer)
        {
            _tooltipDrawer = tooltipDrawer;
        }

        public void SetPerk(PerkSO perk)
        {
            _perk = perk;
            _perkImage.sprite = _perk.Icon;
        }

        protected override void ShowTooltip()
        {
            _tooltipDrawer.ShowTooltip(_perk);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideTooltips();
        }
    }
}
