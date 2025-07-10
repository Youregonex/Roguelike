using UnityEngine;
using System.Collections.Generic;
using Yg.GameData.Equipment;
using Yg.GameData.Configs;
using TMPro;
using Yg.GameData.Perks;

namespace Yg.UI
{
    public class EquipmentTooltipUI : BaseTooltipUI
    {
        [CustomHeader("Settings")]
        [SerializeField] private StatIconConfig _statIconConfig;
        [SerializeField] private TextMeshProUGUI _rarityText;
        [SerializeField] private TextMeshProUGUI _tierText;
        [SerializeField] private List<StatModifierUI> _statModifierList;
        [SerializeField] private List<BonusPerkUI> _bonusPerkUIList;

        public void SetData(EquipmentData equipmentData)
        {
            _tooltipTitleText.text = equipmentData.Name;
            _iconImage.sprite = ResourceLoader.GetIconWithPath(equipmentData.IconPath);
            _rarityText.text = equipmentData.Rarity.ToString();
            _tierText.text = $"Tier: {equipmentData.Tier}";

            ShowStatModifiers(equipmentData);
            ShowBonusPerkUIs(equipmentData);

            Canvas.ForceUpdateCanvases();
        }

        private void ShowStatModifiers(EquipmentData equipmentData)
        {
            Sprite icon;

            for (int i = 0; i < _statModifierList.Count; i++)
            {
                if (i >= equipmentData.StatModifierList.Count)
                    _statModifierList[i].gameObject.SetActive(false);
                else
                {
                    _statModifierList[i].gameObject.SetActive(true);
                    icon = _statIconConfig.GetIcon(equipmentData.StatModifierList[i].StatType);
                    _statModifierList[i].SetData(icon, equipmentData.StatModifierList[i]);
                }
            }
        }

        private void ShowBonusPerkUIs(EquipmentData equipmentData)
        {
            PerkSO perkSO;

            for (int i = 0; i < _bonusPerkUIList.Count; i++)
            {
                if (i >= equipmentData.PerkIdList.Count)
                    _bonusPerkUIList[i].gameObject.SetActive(false);
                else
                {
                    _bonusPerkUIList[i].gameObject.SetActive(true);
                    perkSO = ResourceLoader.GetPerkSO(equipmentData.PerkIdList[i]);

                    _bonusPerkUIList[i].SetData(perkSO);
                }
            }
        }
    }
}