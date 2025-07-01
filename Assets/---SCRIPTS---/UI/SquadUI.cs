using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yg.Character;
using UnityEngine.EventSystems;
using System;


namespace Yg.UI
{
    public class SquadUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<SquadUI> OnSquadClicked;
        public event Action<SquadUI> OnSquadReleased;

        [CustomHeader("Settings")]
        [SerializeField] private Image _squadImage;
        [SerializeField] private TextMeshProUGUI _squadNameText;
        [SerializeField] private TextMeshProUGUI _squadAmountText;
        [SerializeField] private CanvasGroup _canvasGroup;

        private WarbandSlot _warbandSlot;

        public WarbandSlot WarbandSlot => _warbandSlot;

        public void Initialize(WarbandSlot warbandSlot)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _warbandSlot = warbandSlot;
            _squadImage.sprite = warbandSlot.UnitData.UnitIcon;
            _squadNameText.text = warbandSlot.UnitData.UnitName;
            _squadAmountText.text = warbandSlot.SlotSize.ToString();
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnSquadReleased?.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSquadClicked?.Invoke(this);
        }
    }
}
