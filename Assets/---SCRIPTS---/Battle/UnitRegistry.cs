using System;
using System.Collections.Generic;
using Yg.Battle.BattleUnits;
using Yg.Battle.GameSystems;
using Zenject;

namespace Yg.Battle
{
    public class UnitRegistry : IDisposable
    {
        private BattleUnitSpawner _battleUnitSpawner;

        private readonly List<BattleUnitCore> _playerUnitList = new();
        private readonly List<BattleUnitCore> _enemyUnitList = new();

        [Inject]
        private void Construct(BattleUnitSpawner battleUnitSpawner)
        {
            _battleUnitSpawner = battleUnitSpawner;
            _battleUnitSpawner.OnUnitSpawned += BattleUnitSpawner_OnUnitSpawned;
        }

        public void Dispose()
        {
            List<BattleUnitCore> combinedUnits = new(_playerUnitList);
            combinedUnits.AddRange(_enemyUnitList);

            for (int i = 0; i < combinedUnits.Count; i++)
                combinedUnits[i].OnDeath -= BattleUnitCore_OnDeath;
        }

        public IReadOnlyList<BattleUnitCore> GetEnemyList(EUnitFaction unitFaction)
        {
            if (unitFaction == EUnitFaction.Player) return _enemyUnitList;
            else return _playerUnitList;
        }

        public IReadOnlyList<BattleUnitCore> GetAllyList(EUnitFaction unitFaction)
        {
            if (unitFaction == EUnitFaction.Player) return _playerUnitList;
            else return _enemyUnitList;
        }

        private void BattleUnitSpawner_OnUnitSpawned(BattleUnitCore spawnedUnit)
        {
            if (spawnedUnit.UnitFaction == EUnitFaction.Player) _playerUnitList.Add(spawnedUnit);
            else _enemyUnitList.Add(spawnedUnit);
            spawnedUnit.OnDeath += BattleUnitCore_OnDeath;
        }

        private void BattleUnitCore_OnDeath(BattleUnitCore diedUnit)
        {
            diedUnit.OnDeath -= BattleUnitCore_OnDeath;

            if (diedUnit.UnitFaction == EUnitFaction.Player) _playerUnitList.Remove(diedUnit);
            else _enemyUnitList.Remove(diedUnit);
        }
    }
}