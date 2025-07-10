using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Yg.UI
{
    public abstract class TooltipHolderUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        protected TooltipDrawer _tooltipDrawer;

        [Inject]
        private void Construct(TooltipDrawer tooltipDrawer)
        {
            _tooltipDrawer = tooltipDrawer;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltip();
        }

        public abstract void OnPointerExit(PointerEventData eventData);

        protected abstract void ShowTooltip();
    }
}
