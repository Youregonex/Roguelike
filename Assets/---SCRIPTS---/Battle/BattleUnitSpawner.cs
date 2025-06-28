using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yg.Battle.BattleUnits;

namespace Yg.Battle.GameSystems
{
    public class BattleUnitSpawner : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Transform _playerSpawnPointTransform;
        [SerializeField] private Transform _enemySpawnPointTransform;
        [SerializeField] private BattleUnitCore _meleeUnitPrefab;
        [SerializeField] private BattleUnitCore _rangedUnitPrefab;

        private readonly List<BattleUnitCore> _playerUnitList = new();
        private readonly List<BattleUnitCore> _enemyUnitList = new();

        public IEnumerable<BattleUnitCore> PlayerUnits => _playerUnitList;
        public IEnumerable<BattleUnitCore> EnemyUnits => _enemyUnitList;

        public void Initialize()
        {
            //InitialUnitSpawn();
        }

        private void InitialUnitSpawn()
        {
            
        }

        public void StartBattle(int playerMeleeCount, int playerRangedCount, int enemyMeleeCount, int enemyRangedCound)
        {
            Vector2 randomSpawnOffset;

            for (int i = 0; i < playerMeleeCount; i++)
            {
                randomSpawnOffset = new(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-4f, 4f));
                BattleUnitCore battleUnit = Instantiate(_meleeUnitPrefab, (Vector2)_playerSpawnPointTransform.position + randomSpawnOffset, Quaternion.identity);
                battleUnit.OnDeath += BattleUnit_OnDeath;
                battleUnit.Initialize(EUnitFaction.Player);
                _playerUnitList.Add(battleUnit);
            }

            for (int i = 0; i < playerRangedCount; i++)
            {
                randomSpawnOffset = new Vector2(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-4f, 4f)) + new Vector2(-10f, 0f);
                BattleUnitCore battleUnit = Instantiate(_rangedUnitPrefab, (Vector2)_playerSpawnPointTransform.position + randomSpawnOffset, Quaternion.identity);
                battleUnit.OnDeath += BattleUnit_OnDeath;
                battleUnit.Initialize(EUnitFaction.Player);
                _playerUnitList.Add(battleUnit);
            }

            for (int i = 0; i < enemyMeleeCount; i++)
            {
                randomSpawnOffset = new(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-4f, 4f));
                BattleUnitCore battleUnit = Instantiate(_meleeUnitPrefab, (Vector2)_enemySpawnPointTransform.position + randomSpawnOffset, Quaternion.identity);
                battleUnit.OnDeath += BattleUnit_OnDeath;
                battleUnit.Initialize(EUnitFaction.Enemy);
                _enemyUnitList.Add(battleUnit);
            }

            for (int i = 0; i < enemyRangedCound; i++)
            {
                randomSpawnOffset = new Vector2(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(-4f, 4f)) + new Vector2(10f, 0f);
                BattleUnitCore battleUnit = Instantiate(_rangedUnitPrefab, (Vector2)_enemySpawnPointTransform.position + randomSpawnOffset, Quaternion.identity);
                battleUnit.OnDeath += BattleUnit_OnDeath;
                battleUnit.Initialize(EUnitFaction.Enemy);
                _enemyUnitList.Add(battleUnit);
            }

            AssignTargets();
        }

        public void StopBattle()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void BattleUnit_OnDeath(BattleUnitCore battleUnitCore)
        {
            battleUnitCore.OnDeath -= BattleUnit_OnDeath;

            if (battleUnitCore.UnitFaction == EUnitFaction.Player)
            {
                _playerUnitList.Remove(battleUnitCore);

                foreach (var unit in _enemyUnitList)
                    unit.RemoveTarget(battleUnitCore);
            }

            if (battleUnitCore.UnitFaction == EUnitFaction.Enemy)
            {
                _enemyUnitList.Remove(battleUnitCore);

                foreach (var unit in _playerUnitList)
                    unit.RemoveTarget(battleUnitCore);
            }

            Destroy(battleUnitCore.gameObject);
        }

        private void AssignTargets()
        {
            List<BattleUnitCore> allUnitsList = new(_playerUnitList);
            allUnitsList.AddRange(_enemyUnitList);

            for (int i = 0; i < allUnitsList.Count; i++)
            {
                if (allUnitsList[i].UnitFaction == EUnitFaction.Player)
                    allUnitsList[i].AssignTargets(_enemyUnitList);
                else
                    allUnitsList[i].AssignTargets(_playerUnitList);
            }
        }
    }

    public class BattleUnitData
    {
        public string PrefabId;
    }
}
