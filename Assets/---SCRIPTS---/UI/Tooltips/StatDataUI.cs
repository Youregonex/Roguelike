using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Yg.UI
{
    public class StatDataUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _statIcon;
        [SerializeField] private TextMeshProUGUI _statNameAndValueText;

        public void SetData(Sprite icon, string statName, float statValue)
        {
            _statIcon.sprite = icon;
            _statNameAndValueText.text = $"{statName}: {statValue}";
        }
    }
}
