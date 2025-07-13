using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yg.GameData.Equipment;
using System;
using Zenject;
using Yg.Systems;

namespace Yg.UI
{
    public class EquipmentUI : TooltipHolderUI
    {
        public event Action<EquipmentUI> OnHovered;
        public event Action<EquipmentUI> OnHoverEnd;

        [CustomHeader("Settings")]
        [SerializeField] private Image _itemIconImage;
        [SerializeField] private Image _iconBackgroundImage;

        private WarbandSlotUI _warbandSlotUI;
        private EquipmentData _equipmentData;
        private ColorPicker _colorPicker;

        public WarbandSlotUI WarbandSlotUI => _warbandSlotUI;
        public EquipmentData EquipmentData => _equipmentData;

        [Inject]
        private void Construct(ColorPicker colorPicker)
        {
            _colorPicker = colorPicker;
        }

        public void Initialize(WarbandSlotUI warbandSlotUI)
        {
            _warbandSlotUI = warbandSlotUI;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            OnHovered?.Invoke(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideTooltips();
            OnHoverEnd?.Invoke(this);
        }

        public void SetData(EquipmentData equipmentData)
        {
            _equipmentData = equipmentData;
            _itemIconImage.sprite = equipmentData?.Icon;

            if (_itemIconImage.sprite == null) _itemIconImage.color = new Color(1f, 1f, 1f, 0f);
            else _itemIconImage.color = new Color(1f, 1f, 1f, 1f);

            _iconBackgroundImage.color =
                _equipmentData == null ? Color.white : _colorPicker.GetColor(_equipmentData.Rarity);
        }

        protected override void ShowTooltip()
        {
            if (_equipmentData == null) return;
            _tooltipDrawer.ShowTooltip(_equipmentData);
        }
    }
}