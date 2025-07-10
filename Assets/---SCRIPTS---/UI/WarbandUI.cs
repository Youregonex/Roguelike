using System.Collections.Generic;
using UnityEngine;
using Yg.Character;
using DG.Tweening;
using Zenject;
using System.Collections;
using UnityEngine.UI;

namespace Yg.UI
{
    public class WarbandUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _warbandWindowRectTransform;
        [SerializeField] private RectTransform _warbandSlotUIHolder;
        [SerializeField] private WarbandSlotUI _warbandSlotUIPrefab;
        [SerializeField] private PerkUI _perkUIPrefab;
        [SerializeField] private SpellUI _spellUIPrefab;

        [CustomHeader("DOTween Settings")]
        [SerializeField] private float _scaleTargetValue = 1.3f;
        [SerializeField] private float _animationDuration = .5f;

        private DiContainer _container;

        private List<WarbandSlotUI> _warbandSlotUIList = new();

        public IEnumerable<WarbandSlotUI> WarbandSlotUIList => _warbandSlotUIList;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        public void CreateWarbandSlotUI(WarbandSlot warbandSlot)
        {
            WarbandSlotUI warbandSlotUI = _container.InstantiatePrefab(_warbandSlotUIPrefab, _warbandSlotUIHolder).GetComponent<WarbandSlotUI>();
            _warbandSlotUIList.Add(warbandSlotUI);
            warbandSlotUI.AssignWarbandSlot(warbandSlot);
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