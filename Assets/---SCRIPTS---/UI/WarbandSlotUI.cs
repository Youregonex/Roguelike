using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Yg.Character;
using UnityEngine.EventSystems;
using System;

namespace Yg.UI
{
    public class WarbandSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action<WarbandSlotUI> OnSelected;

        [CustomHeader("Settings")]
        [SerializeField] private Image _unitImage;
        [SerializeField] private Image _selectionBackground;
        [SerializeField] private TextMeshProUGUI _squadSizeText;
        [SerializeField] private Sprite _defaultSprite;

        private WarbandSlot _warbandSlot;

        public WarbandSlot WarbandSlot => _warbandSlot;

        private void Awake()
        {
            _selectionBackground.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSelected?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _selectionBackground.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _selectionBackground.gameObject.SetActive(false);
        }

        public void SetSlotData(WarbandSlot warbandSlot)
        {
            _warbandSlot = warbandSlot;
            UpdateSlotData();
        }

        public void UpdateSlotData()
        {
            if (_warbandSlot.Empty)
            {
                _unitImage.sprite = _defaultSprite;
                _squadSizeText.text = "";
            }
            else
            {
                _unitImage.sprite = _warbandSlot.UnitData.UnitIcon;
                _squadSizeText.text = _warbandSlot.SlotSize.ToString();
            } 
        }
    }
}
