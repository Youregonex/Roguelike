using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yg.Battle;
using Zenject;

namespace Yg.UI
{
    public class SpellUI : TooltipHolderUI
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _spellImage;

        private SpellSO _spellSO;

        [Inject]
        private void Construct(TooltipDrawer tooltipDrawer)
        {
            _tooltipDrawer = tooltipDrawer;
        }

        public void SetSpell(SpellSO spellSO)
        {
            _spellSO = spellSO;
            _spellImage.sprite = _spellSO.Icon;
        }

        protected override void ShowTooltip()
        {
            _tooltipDrawer.ShowTooltip(_spellSO);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideTooltips();
        }
    }
}