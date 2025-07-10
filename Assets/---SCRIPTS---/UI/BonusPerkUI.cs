using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yg.GameData.Perks;

namespace Yg.UI
{
    public class BonusPerkUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Image _perkIconImage;
        [SerializeField] private TextMeshProUGUI _perkTitleText;
        [SerializeField] private TextMeshProUGUI _perkDescriptionText;

        public void SetData(PerkSO perkSO)
        {
            _perkIconImage.sprite = perkSO.Icon;
            _perkTitleText.text = $"Squad gains {perkSO.Name} perk:";
            _perkDescriptionText.text = perkSO.Description;
        }
    }
}
