using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yg.GameData.Perks;
using Zenject;

namespace Yg.UI
{
    public class PerkUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _perkImage;

        private Perk _perk;
        private TooltipDrawer _tooltipDrawer;

        [Inject]
        private void Construct(TooltipDrawer tooltipDrawer)
        {
            _tooltipDrawer = tooltipDrawer;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltipDrawer.ShowTooltip(_perk, (RectTransform)transform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideTooltip();
        }

        public void SetPerk(Perk perk)
        {
            _perk = perk;
            _perkImage.sprite = _perk.PerkIcon;
        }
    }
}
