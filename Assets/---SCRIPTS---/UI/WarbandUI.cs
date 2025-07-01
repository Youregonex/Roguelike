using System.Collections.Generic;
using UnityEngine;
using Yg.Character;
using DG.Tweening;

namespace Yg.UI
{
    public class WarbandUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _warbandWindowRectTransform;
        [SerializeField] private RectTransform _warbandSlotUIHolder;
        [SerializeField] private WarbandSlotUI _warbandSlotUIPrefab;

        [CustomHeader("DOTween Settings")]
        [SerializeField] private float _scaleTargetValue = 1.3f;
        [SerializeField] private float _animationDuration = .5f;

        private List<WarbandSlotUI> _warbandSlotUIList = new();

        public IEnumerable<WarbandSlotUI> WarbandSlotUIList => _warbandSlotUIList;

        public void CreateWarbandSlotUI(WarbandSlot warbandSlot)
        {
            WarbandSlotUI warbandSlotUI = Instantiate(_warbandSlotUIPrefab);
            warbandSlotUI.transform.SetParent(_warbandSlotUIHolder);
            _warbandSlotUIList.Add(warbandSlotUI);
            warbandSlotUI.SetSlotData(warbandSlot);
        }

        public void UpdateSlotsData()
        {
            foreach (var warbandSlotUI in _warbandSlotUIList)
                warbandSlotUI.UpdateSlotData();
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

