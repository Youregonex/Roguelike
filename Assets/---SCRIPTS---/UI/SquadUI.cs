using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yg.Character;
using UnityEngine.EventSystems;
using System;

namespace Yg.UI
{
    public class SquadUI : MonoBehaviour, IPointerDownHandler
    {
        public event Action<SquadUI> OnSquadClicked;

        [CustomHeader("Settings")]
        [SerializeField] private Image _squadImage;
        [SerializeField] private TextMeshProUGUI _squadNameText;
        [SerializeField] private TextMeshProUGUI _squadAmountText;

        private WarbandSlot _warbandSlot;

        public WarbandSlot WarbandSlot => _warbandSlot;

        public void Initialize(WarbandSlot warbandSlot)
        {
            _warbandSlot = warbandSlot;
            _squadImage.sprite = warbandSlot.UnitData.Icon;
            _squadNameText.text = warbandSlot.UnitData.Name;
            _squadAmountText.text = warbandSlot.SlotSize.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSquadClicked?.Invoke(this);
        }
    }
}
