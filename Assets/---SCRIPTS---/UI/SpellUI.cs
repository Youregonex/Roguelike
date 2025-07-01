using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yg.Battle;
using Zenject;

namespace Yg.UI
{
    public class SpellUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _spellImage;

        private SpellSO _spellSO;
        private TooltipDrawer _tooltipDrawer;

        [Inject]
        private void Construct(TooltipDrawer tooltipDrawer)
        {
            _tooltipDrawer = tooltipDrawer;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltipDrawer.ShowTooltip(_spellSO, (RectTransform)transform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideTooltip();
        }

        public void SetSpell(SpellSO spellSO)
        {
            _spellSO = spellSO;
            _spellImage.sprite = _spellSO.Icon;
        }
    }
}