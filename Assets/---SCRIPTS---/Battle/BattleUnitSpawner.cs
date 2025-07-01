using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yg.Battle.BattleUnits;
using Yg.Character;
using Yg.GameData;
using Yg.GameData.Units;
using Yg.UI;
using Zenject;

namespace Yg.Battle.GameSystems
{
    public class BattleUnitSpawner : MonoBehaviour
    {
        private const string GAMEPLAY_SCENE_NAME = "Gameplay";

        [CustomHeader("Settings")]
        [SerializeField] private Transform _playerSpawnPointTransform;
        [SerializeField] private Transform _enemySpawnPointTransform;
        [SerializeField] private BattleUnitCore _meleeUnitPrefab;
        [SerializeField] private BattleUnitCore _rangedUnitPrefab;
        [SerializeField] private List<SquadPlacementArea> _playerSquadPlacementAreas;
        [SerializeField] private List<SquadPlacementArea> _enemySquadPlacementAreas;
        [SerializeField] private bool _TEST;

        private SquadPlacementUI _squadPlacementUI;

        private readonly List<BattleUnitCore> _playerUnitList = new();
        private readonly List<BattleUnitCore> _enemyUnitList = new();

        public IEnumerable<BattleUnitCore> PlayerUnits => _playerUnitList;
        public IEnumerable<BattleUnitCore> EnemyUnits => _enemyUnitList;

        [Inject]
        private void Construct(SquadPlacementUI squadPlacementUI)
        {
            _squadPlacementUI = squadPlacementUI;
            _squadPlacementUI.OnTroopsReady += SquadPlacementUI_OnTroopsReady;
        }

        private void SquadPlacementUI_OnTroopsReady()
        {
            _squadPlacementUI.OnTroopsReady -= SquadPlacementUI_OnTroopsReady;

            SpawnPlayerTroops();
            SpawnEnemyTroops();

            AssignTargets();
        }

        public void Initialize(PersistentData persistentData)
        {
            
        }

        private void SpawnPlayerTroops()
        {
            for (int i = 0; i < _playerSquadPlacementAreas.Count; i++)
            {
                if (_playerSquadPlacementAreas[i].Empty) continue;
                SpawnSquad(_playerSquadPlacementAreas[i].SquadUI.WarbandSlot, _playerSquadPlacementAreas[i], EUnitFaction.Player);
            }

            for (int i = 0; i < _playerSquadPlacementAreas.Count; i++)
                _playerSquadPlacementAreas[i].gameObject.SetActive(false);
        }

        private void SpawnEnemyTroops()
        {
            for (int i = 0; i < _enemySquadPlacementAreas.Count; i++)
            {
                if (_enemySquadPlacementAreas[i].Empty) continue;
                SpawnSquad(_enemySquadPlacementAreas[i].WarbandSlot, _enemySquadPlacementAreas[i], EUnitFaction.Enemy);
            }

            for (int i = 0; i < _enemySquadPlacementAreas.Count; i++)
                _enemySquadPlacementAreas[i].gameObject.SetActive(false);
        }

        private void SpawnSquad(WarbandSlot warbandSlot, SquadPlacementArea squadPlacementArea, EUnitFaction unitFaction)
        {
            Vector2 spawnPosition;
            float randomPositionX;
            float randomPositionY;
            Bounds bounds;

            for (int i = 0; i < warbandSlot.SlotSize; i++)
            {
                bounds = squadPlacementArea.Collider.bounds;
                randomPositionX = Random.Range(bounds.min.x, bounds.max.x);
                randomPositionY = Random.Range(bounds.min.y, bounds.max.y);

                spawnPosition = new(randomPositionX, randomPositionY);

                SpawnUnit(warbandSlot.UnitData, spawnPosition, unitFaction);
            }
        }

        private void SpawnUnit(UnitDataSO unitDataSO, Vector2 spawnPosition, EUnitFaction unitFaction)
        {
            BattleUnitCore battleUnit = Instantiate(unitDataSO.UnitPrefab, spawnPosition, Quaternion.identity);
            battleUnit.OnDeath += BattleUnit_OnDeath;
            battleUnit.Initialize(unitFaction);

            if (unitFaction == EUnitFaction.Player)
                _playerUnitList.Add(battleUnit);
            else
                _enemyUnitList.Add(battleUnit);
        }

        public void StartBattleTEST(int playerMeleeCount, int playerRangedCount, int enemyMeleeCount, int enemyRangedCound)
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

        public void StopBattleTEST()
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

            if (!_TEST && (_playerUnitList.Count == 0 || _enemyUnitList.Count == 0))
                SceneManager.LoadScene(GAMEPLAY_SCENE_NAME);
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
}
