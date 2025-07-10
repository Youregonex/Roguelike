using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yg.GameData.Units;

namespace Yg.UI
{
    public class UnitTooltipUI : BaseTooltipUI
    {
        [CustomHeader("Unit Tooltip settings")]
        [SerializeField] private RectTransform _statHolder;
        [SerializeField] private StatDataUI _statDataPrefab;
        [SerializeField] private List<Image> _perkIconList;
        [SerializeField] private List<Image> _spellIconList;

        private readonly List<StatDataUI> _statDataList = new();

        private bool _statsInitialized = false;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetData(UnitDataSO unitDataSO)
        {
            SetData(unitDataSO.Name, "", unitDataSO.Icon);

            if (!_statsInitialized) SetupStats(unitDataSO);
            else UpdateStatVisuals(unitDataSO);

            UpdatePerkVisuals(unitDataSO);
            UpdateSpellVisuals(unitDataSO);
        }

        private void SetupStats(UnitDataSO unitDataSO)
        {
            for (int i = 0; i < unitDataSO.UnitStatDataList.Count; i++)
            {
                StatDataUI statDataUI = Instantiate(_statDataPrefab, _statHolder);
                statDataUI.SetData(
                    unitDataSO.UnitStatDataList[i].Icon,
                    unitDataSO.UnitStatDataList[i].Name,
                    unitDataSO.UnitStatDataList[i].MaxValue);

                _statDataList.Add(statDataUI);
            }

            _statsInitialized = true;
        }

        private void UpdateStatVisuals(UnitDataSO unitDataSO)
        {
            for (int i = 0; i < _statDataList.Count; i++)
            {
                _statDataList[i].SetData(
                    unitDataSO.UnitStatDataList[i].Icon,
                    unitDataSO.UnitStatDataList[i].Name,
                    unitDataSO.UnitStatDataList[i].MaxValue);
            }
        }

        private void UpdatePerkVisuals(UnitDataSO unitDataSO)
        {
            for (int i = 0; i < _perkIconList.Count; i++)
            {
                if (i >= unitDataSO.PerkSOList.Count)
                    _perkIconList[i].gameObject.SetActive(false);
                else
                {
                    _perkIconList[i].gameObject.SetActive(true);
                    _perkIconList[i].sprite = unitDataSO.PerkSOList[i].Icon;
                }
            }
        }

        private void UpdateSpellVisuals(UnitDataSO unitDataSO)
        {
            for (int i = 0; i < _spellIconList.Count; i++)
            {
                if (i >= unitDataSO.SpellSOList.Count)
                    _spellIconList[i].gameObject.SetActive(false);
                else
                {
                    _spellIconList[i].gameObject.SetActive(true);
                    _spellIconList[i].sprite = unitDataSO.SpellSOList[i].Icon;
                }
            }
        }
    }
}
