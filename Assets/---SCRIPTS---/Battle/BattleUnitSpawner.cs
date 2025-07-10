using System.Collections.Generic;
using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Character;
using Yg.GameData;
using Yg.GameData.Units;
using Yg.UI;
using Zenject;
using System;
using Yg.Factories;

namespace Yg.Battle.GameSystems
{
    public class BattleUnitSpawner : MonoBehaviour
    {
        public event Action OnUnitSpawnComplete;
        public event Action<BattleUnitCore> OnUnitSpawned;

        [CustomHeader("Settings")]
        [SerializeField] private List<SquadPlacementArea> _playerSquadPlacementAreas;
        [SerializeField] private List<SquadPlacementArea> _enemySquadPlacementAreas;

        private SquadPlacementUI _squadPlacementUI;
        private BattleUnitFactory _battleUnitFactory;

        [Inject]
        private void Construct(SquadPlacementUI squadPlacementUI, BattleUnitFactory battleUnitFactory)
        {
            _squadPlacementUI = squadPlacementUI;
            _squadPlacementUI.OnTroopsReady += SquadPlacementUI_OnTroopsReady;

            _battleUnitFactory = battleUnitFactory;
        }

        private void SquadPlacementUI_OnTroopsReady()
        {
            _squadPlacementUI.OnTroopsReady -= SquadPlacementUI_OnTroopsReady;

            SpawnTroops(EUnitFaction.Player);
            SpawnTroops(EUnitFaction.Enemy);

            OnUnitSpawnComplete?.Invoke();
        }

        public void Initialize(PersistentData persistentData)
        {
            
        }

        private void SpawnTroops(EUnitFaction unitFaction)
        {
            List<SquadPlacementArea> placementAreaList;

            if (unitFaction == EUnitFaction.Player) placementAreaList = _playerSquadPlacementAreas;
            else placementAreaList = _enemySquadPlacementAreas;

            for (int i = 0; i < placementAreaList.Count; i++)
            {
                if (placementAreaList[i].Empty)
                {
                    placementAreaList[i].gameObject.SetActive(false);
                    continue;
                }

                SpawnSquad(placementAreaList[i].WarbandSlot, placementAreaList[i], unitFaction);
                placementAreaList[i].gameObject.SetActive(false);
            }
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
                randomPositionX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
                randomPositionY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

                spawnPosition = new(randomPositionX, randomPositionY);

                SpawnUnit(warbandSlot.UnitData, spawnPosition, unitFaction);
            }
        }

        private void SpawnUnit(UnitDataSO unitDataSO, Vector2 spawnPosition, EUnitFaction unitFaction)
        {
            BattleUnitCore battleUnit = _battleUnitFactory.CreateUnit(unitDataSO);
            battleUnit.transform.position = spawnPosition;
            battleUnit.Initialize(unitFaction);

            OnUnitSpawned?.Invoke(battleUnit);
        }
    }
}