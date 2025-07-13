using System.Collections.Generic;
using UnityEngine;
using Yg.Character;
using DG.Tweening;
using Zenject;
using System;
using UnityEngine.UI;

namespace Yg.UI
{
    public class WarbandUI : MonoBehaviour
    {
        public event Action<EquipmentUI> OnEquipmentUIHovered;
        public event Action<EquipmentUI> OnEquipmentUIHoverEnd;

        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _warbandWindowRectTransform;
        [SerializeField] private RectTransform _warbandSlotUIHolder;
        [SerializeField] private Image _mouseObject;
        [SerializeField] private WarbandSlotUI _warbandSlotUIPrefab;
        [SerializeField] private PerkUI _perkUIPrefab;
        [SerializeField] private SpellUI _spellUIPrefab;

        [CustomHeader("DOTween Settings")]
        [SerializeField] private float _scaleTargetValue = 1.3f;
        [SerializeField] private float _animationDuration = .5f;

        private DiContainer _container;
        private List<WarbandSlotUI> _warbandSlotUIList = new();
        private Vector3 _mouseObjectPosition;

        public IEnumerable<WarbandSlotUI> WarbandSlotUIList => _warbandSlotUIList;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        private void Awake()
        {
            DisableMouseObject();
        }

        private void Update()
        {
            if (!_mouseObject.gameObject.activeInHierarchy) return;

            _mouseObjectPosition = Input.mousePosition;
            _mouseObjectPosition.z = 0f;
            _mouseObject.transform.position = _mouseObjectPosition;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _warbandSlotUIList.Count; i++)
            {
                _warbandSlotUIList[i].OnEquipmentSlotHovered -= WarbandSlotUI_OnEquipmentSlotHovered;
                _warbandSlotUIList[i].OnEquipmentSlotHoverEnd -= WarbandSlotUI_OnEquipmentSlotHoverEnd;
            }
        }

        public void EnableMouseObject(Sprite sprite)
        {
            _mouseObject.gameObject.SetActive(true);
            _mouseObject.sprite = sprite;
        }

        public void DisableMouseObject()
        {
            _mouseObject.gameObject.SetActive(false);
        }

        public void CreateWarbandSlotUIs(WarbandSlot warbandSlot)
        {
            WarbandSlotUI warbandSlotUI = _container
                .InstantiatePrefabForComponent<WarbandSlotUI>(_warbandSlotUIPrefab, _warbandSlotUIHolder);

            _warbandSlotUIList.Add(warbandSlotUI);
            warbandSlotUI.AssignWarbandSlot(warbandSlot);

            warbandSlotUI.OnEquipmentSlotHovered += WarbandSlotUI_OnEquipmentSlotHovered;
            warbandSlotUI.OnEquipmentSlotHoverEnd += WarbandSlotUI_OnEquipmentSlotHoverEnd;
        }

        private void WarbandSlotUI_OnEquipmentSlotHovered(EquipmentUI equipmentUI)
        {
            OnEquipmentUIHovered?.Invoke(equipmentUI);
        }

        private void WarbandSlotUI_OnEquipmentSlotHoverEnd(EquipmentUI equipmentUI)
        {
            OnEquipmentUIHoverEnd?.Invoke(equipmentUI);
        }

        public void UpdateSlotsUnitData()
        {
            foreach (var warbandSlotUI in _warbandSlotUIList)
                warbandSlotUI.UpdateUnitData();
        }

        public void UpdateSlotsEquipmentData()
        {
            foreach (var warbandSlotUI in _warbandSlotUIList)
                warbandSlotUI.UpdateEquipmentData();
        }

        public void Show()
        {
            _warbandWindowRectTransform.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _warbandWindowRectTransform.gameObject.SetActive(false);
        }

        public void ScaleUp()
        {
            _warbandWindowRectTransform.DOScale(_scaleTargetValue, _animationDuration);
        }

        public void ScaleDown()
        {
            _warbandWindowRectTransform.DOScale(1f, _animationDuration);
        }
    }
}