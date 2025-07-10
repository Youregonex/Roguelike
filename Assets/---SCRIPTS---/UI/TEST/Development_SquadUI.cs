using UnityEngine;
using Yg.GameData.Units;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace Yg.UI
{
    public class Development_SquadUI : MonoBehaviour, IPointerDownHandler
    {
        public event Action<UnitDataSO> OnSquadCreationRequested;

        [CustomHeader("Settings")]
        [SerializeField] private Image _squadImage;

        private UnitDataSO _unitDataSO;

        public void AssignData(UnitDataSO unitDataSO)
        {
            _unitDataSO = unitDataSO;
            _squadImage.sprite = unitDataSO.Icon;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSquadCreationRequested?.Invoke(_unitDataSO);
        }
    }
}
