using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yg.GameData.Units;
using Zenject;
using Yg.Battle.GameSystems;
using Yg.Battle.BattleUnits;

namespace Yg.UI
{
    public class PlayerUnitDamageOutputUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _squadDamageUIHolder;
        [SerializeField] private SquadDamageUI _squadDamageUIPrefab;
        [SerializeField] private TextMeshProUGUI _totalDamageText;

        private float _totalDamage;

        private BattleUnitSpawner _battleUnitSpawner;

        private Dictionary<UnitDataSO, SquadDamageUI> _unitDamageUIDictionary = new();

        [Inject]
        private void Construct(BattleUnitSpawner battleUnitSpawner)
        {
            _battleUnitSpawner = battleUnitSpawner;
            _battleUnitSpawner.OnUnitSpawnComplete += BattleUnitSpawner_OnUnitSpawnComplete;
        }

        private void Awake()
        {
            _totalDamage = 0f;
            _totalDamageText.text = _totalDamage.ToString();
        }

        private void OnDestroy()
        {
            foreach (var unit in _battleUnitSpawner.PlayerUnits)
                unit.OnDamageDealt -= Unit_OnDamageDealt;
            
            _battleUnitSpawner.OnUnitSpawnComplete -= BattleUnitSpawner_OnUnitSpawnComplete;
        }

        private void BattleUnitSpawner_OnUnitSpawnComplete()
        {
            foreach (var unit in _battleUnitSpawner.PlayerUnits)
                unit.OnDamageDealt += Unit_OnDamageDealt;
        }

        private void Unit_OnDamageDealt(BattleUnitCore unit, float damageDealt)
        {
            _totalDamage += damageDealt;
            _totalDamageText.text = _totalDamage.ToString("F2");

            if(_unitDamageUIDictionary.ContainsKey(unit.UnitData))
                _unitDamageUIDictionary[unit.UnitData].AddDamage(damageDealt, _totalDamage);
            else
            {
                SquadDamageUI squadDamageUI = Instantiate(_squadDamageUIPrefab, _squadDamageUIHolder);
                _unitDamageUIDictionary.Add(unit.UnitData, squadDamageUI);
                squadDamageUI.Initialize(unit.UnitData.UnitIcon);
                squadDamageUI.AddDamage(damageDealt, _totalDamage);
            }

            foreach (var squadDamageUI in _unitDamageUIDictionary)
                squadDamageUI.Value.UpdateFillAmount(_totalDamage);
        }
    }
}
