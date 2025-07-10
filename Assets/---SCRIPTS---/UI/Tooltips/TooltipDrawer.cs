using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yg.Battle;
using Yg.GameData.Equipment;
using Yg.GameData.Perks;
using Yg.GameData.Units;

namespace Yg.UI
{
    public class TooltipDrawer : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _tooltipPosition;
        [SerializeField] private BaseTooltipUI _baseTooltipPrefab;
        [SerializeField] private SpellTooltipUI _spellTooltipPrefab;
        [SerializeField] private EquipmentTooltipUI _equipmentTooltipPrefab;
        [SerializeField] private UnitTooltipUI _unitTooltip;

        private readonly List<BaseTooltipUI> _activeTooltips = new();
        private readonly List<BaseTooltipUI> _perkTooltipList = new();
        private readonly List<SpellTooltipUI> _spellTooltipList = new();
        private readonly List<EquipmentTooltipUI> _equipmentTooltipList = new();

        private void Awake()
        {
            _baseTooltipPrefab.gameObject.SetActive(false);
        }

        public void ShowTooltip(UnitDataSO unitDataSO)
        {
            _unitTooltip.SetData(unitDataSO);
            _unitTooltip.gameObject.SetActive(true);
            _unitTooltip.Show();
        }

        public void ShowTooltip(PerkSO perkSO)
        {
            if (_perkTooltipList.Count == 0)
            {
                BaseTooltipUI perkTooltip = Instantiate(_baseTooltipPrefab, transform);
                _perkTooltipList.Add(perkTooltip);
            }

            _perkTooltipList[0].gameObject.SetActive(true);
            _perkTooltipList[0].SetData(perkSO.Name, perkSO.Description, perkSO.Icon);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_perkTooltipList[0].GetComponent<RectTransform>());
            _perkTooltipList[0].transform.position = _tooltipPosition.position;
            _perkTooltipList[0].Show();
            _activeTooltips.Add(_perkTooltipList[0]);
        }

        public void ShowTooltip(SpellSO spellSO)
        {
            if(_spellTooltipList.Count == 0)
            {
                SpellTooltipUI spellTooltipUI = Instantiate(_spellTooltipPrefab, transform);
                _spellTooltipList.Add(spellTooltipUI);
            }

            _spellTooltipList[0].gameObject.SetActive(true);
            _spellTooltipList[0].SetData(spellSO);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_spellTooltipList[0].GetComponent<RectTransform>());
            _spellTooltipList[0].transform.position = _tooltipPosition.position;
            _spellTooltipList[0].Show();
            _activeTooltips.Add(_spellTooltipList[0]);
        }

        public void ShowTooltip(EquipmentData equipmentData)
        {
            if (_equipmentTooltipList.Count == 0)
            {
                EquipmentTooltipUI equipmentTooltip = Instantiate(_equipmentTooltipPrefab, transform);
                _equipmentTooltipList.Add(equipmentTooltip);
            }

            _equipmentTooltipList[0].gameObject.SetActive(true);
            _equipmentTooltipList[0].SetData(equipmentData);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_equipmentTooltipList[0].GetComponent<RectTransform>());
            _equipmentTooltipList[0].transform.position = _tooltipPosition.position;
            _equipmentTooltipList[0].Show();
            _activeTooltips.Add(_equipmentTooltipList[0]);
        }

        public void HideTooltips()
        {
            for (int i = 0; i < _activeTooltips.Count; i++)
                _activeTooltips[i].Hide();

            _activeTooltips.Clear();
        }

        public void HideUnitTooltip()
        {
            _unitTooltip.Hide();
        }
    }
}