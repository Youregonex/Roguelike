using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Yg.Character;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Zenject;

namespace Yg.UI
{
    public class WarbandSlotUI : TooltipHolderUI, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action<WarbandSlotUI> OnSelected;
        public event Action<EquipmentUI> OnEquipmentSlotHovered;
        public event Action<EquipmentUI> OnEquipmentSlotHoverEnd;

        [CustomHeader("Settings")]
        [SerializeField] private Image _unitImage;
        [SerializeField] private Image _selectionBackground;
        [SerializeField] private TextMeshProUGUI _squadSizeText;
        [SerializeField] private TextMeshProUGUI _squadNameText;
        [SerializeField] private Sprite _defaultSprite;

        [SerializeField] private List<EquipmentUI> _equipmentUIList;
        [SerializeField] private List<PerkUI> _perkUIList;
        [SerializeField] private List<SpellUI> _spellUIList;

        //private DiContainer _container;
        private WarbandSlot _warbandSlot;

        public WarbandSlot WarbandSlot => _warbandSlot;

        //[Inject]
        //private void Construct(DiContainer container)
        //{
        //    _container = container;
        //}
        
        private void Awake()
        {
            _selectionBackground.gameObject.SetActive(false);

            DisablePerkUIs();
            DisableSpellUIs();
            InitializeEquipmentUIs();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _equipmentUIList.Count; i++)
            {
                _equipmentUIList[i].OnHovered -= WarbandSlotUI_OnHovered;
                _equipmentUIList[i].OnHoverEnd -= WarbandSlotUI_OnHoverEnd;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _selectionBackground.gameObject.SetActive(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideUnitTooltip();
            _selectionBackground.gameObject.SetActive(false);
        }

        public int GetIndexOfEquipmentUI(EquipmentUI equipmentUI)
        {
            return _equipmentUIList.IndexOf(equipmentUI);
        }

        public void AssignWarbandSlot(WarbandSlot warbandSlot)
        {
            _warbandSlot = warbandSlot;

            UpdateUnitSLotData();
            UpdateEquipmentSlotData();

            Canvas.ForceUpdateCanvases(); // Recalculate WarbandSlotUI structure (for parent's proper auto-sizing)
        }

        public void UpdateUnitData()
        {
            UpdateUnitSLotData();
        }

        public void UpdateEquipmentData()
        {
            UpdateEquipmentSlotData();
        }

        private void UpdateUnitSLotData()
        {
            if (_warbandSlot.UnitEmpty) SetEmptyUnitState();
            else SetUnitState();
        }

        private void UpdateEquipmentSlotData()
        {
            if (_warbandSlot.EquipmentDataList == null) return;
            UpdateEquipmentUIs();
        }

        private void SetEmptyUnitState()
        {
            _unitImage.sprite = _defaultSprite;
            _squadSizeText.text = "";
            _squadNameText.text = "";

            DisablePerkUIs();
            DisableSpellUIs();
        }

        private void SetUnitState()
        {
            _unitImage.sprite = _warbandSlot.UnitData.Icon;
            _squadSizeText.text = _warbandSlot.SlotSize.ToString();
            _squadNameText.text = _warbandSlot.UnitData.Name;

            UpdatePerkUIs();
            UpdateSpellUIs();
        }

        private void UpdatePerkUIs()
        {
            for (int i = 0; i < _perkUIList.Count; i++)
            {
                if (i >= _warbandSlot.UnitData.PerkSOList.Count)
                {
                    _perkUIList[i].gameObject.SetActive(false);
                    continue;
                }

                _perkUIList[i].gameObject.SetActive(true);
                _perkUIList[i].SetPerk(_warbandSlot.UnitData.PerkSOList[i]);
            }
        }

        private void UpdateSpellUIs()
        {
            for (int i = 0; i < _spellUIList.Count; i++)
            {
                if (i >= _warbandSlot.UnitData.SpellSOList.Count)
                {
                    _spellUIList[i].gameObject.SetActive(false);
                    continue;
                }

                _spellUIList[i].gameObject.SetActive(true);
                _spellUIList[i].SetSpell(_warbandSlot.UnitData.SpellSOList[i]);
            }
        }

        private void UpdateEquipmentUIs()
        {
            if (_warbandSlot.EquipmentDataList == null) return;

            for (int i = 0; i < _warbandSlot.EquipmentDataList.Count; i++)
            {
                //_equipmentUIList[i].gameObject.SetActive(true);
                _equipmentUIList[i].SetData(_warbandSlot.EquipmentDataList[i]);
            }
        }

        private void DisablePerkUIs()
        {
            for (int i = 0; i < _perkUIList.Count; i++)
                _perkUIList[i].gameObject.SetActive(false);
        }

        private void DisableSpellUIs()
        {
            for (int i = 0; i < _spellUIList.Count; i++)
                _spellUIList[i].gameObject.SetActive(false);
        }

        private void InitializeEquipmentUIs()
        {
            for (int i = 0; i < _equipmentUIList.Count; i++)
            {
                _equipmentUIList[i].Initialize(this);
                _equipmentUIList[i].OnHovered += WarbandSlotUI_OnHovered;
                _equipmentUIList[i].OnHoverEnd += WarbandSlotUI_OnHoverEnd;
            }
        }

        private void WarbandSlotUI_OnHoverEnd(EquipmentUI equipmentUI)
        {
            OnEquipmentSlotHoverEnd?.Invoke(equipmentUI);
        }

        private void WarbandSlotUI_OnHovered(EquipmentUI equipmentUI)
        {
            OnEquipmentSlotHovered?.Invoke(equipmentUI);
        }

        protected override void ShowTooltip()
        {
            if (_warbandSlot.UnitEmpty) return;

            _tooltipDrawer.ShowTooltip(_warbandSlot.UnitData);
        }
    }
}