using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yg.GameData.Equipment;

namespace Yg.UI
{
    public class EquipmentUI : TooltipHolderUI
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _itemIconImage;
        [SerializeField] private Image _IconBackgroundImage;

        private EquipmentData _equipmentData;

        public override void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideTooltips();
        }

        public void SetData(EquipmentData equipmentData)
        {
            _equipmentData = equipmentData;
            _itemIconImage.sprite = ResourceLoader.GetIconWithPath(_equipmentData.IconPath);
        }

        protected override void ShowTooltip()
        {
            _tooltipDrawer.ShowTooltip(_equipmentData);
        }
    }
}