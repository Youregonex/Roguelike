using System.Collections.Generic;
using UnityEngine;
using Yg.GameData;
using Zenject;
using System.Linq;
using Yg.Character;
using Yg.UI;

namespace Yg.Battle
{
    public class BattleSquadPlacer : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private List<SquadPlacementArea> _enemyPlacementAreas;

        private PersistentData _persistentData;
        private SquadPlacementUI _squadPlacementUI;

        private int _lastPlacementIndex;

        [Inject]
        private void Construct(PersistentData persistentData, SquadPlacementUI squadPlacementUI)
        {
            _persistentData = persistentData;
            _squadPlacementUI = squadPlacementUI;

            _squadPlacementUI.OnAutoPlaceRequired += SquadPlacementUI_OnAutoPlaceRequired;
        }

        public void Initialize()
        {
            PlaceEnemySquads();
        }

        private void OnDestroy()
        {
            _squadPlacementUI.OnAutoPlaceRequired -= SquadPlacementUI_OnAutoPlaceRequired;
        }

        private void SquadPlacementUI_OnAutoPlaceRequired(List<WarbandSlot> warband, List<SquadPlacementArea> placementAreas)
        {
            PlaceSquads(warband, placementAreas);
        }

        private void PlaceEnemySquads()
        {
            List<WarbandSlot> enemyWarband = _persistentData.BattleTransitionData.EnemyWarband;
            PlaceSquads(enemyWarband, _enemyPlacementAreas);
            FlipEnemySquadsSpriteX();
        }

        private void PlaceSquads(List<WarbandSlot> warband, List<SquadPlacementArea> placementAreas)
        {
            placementAreas = placementAreas.OrderBy(e => Vector2.Distance(e.transform.position, Vector2.zero)).ToList();

            List<WarbandSlot> sortedWarband = warband
                .Where(e => e.UnitData != null)
                .OrderBy(e => e.UnitData.AttackType) // Assumes EAttackType.Melee < EAttackType.Ranged
                .ToList();

            if (sortedWarband.Count > placementAreas.Count)
            {
                Debug.LogError("Not enough placement areas for squads!");
                return;
            }

            for (int i = 0; i < sortedWarband.Count; i++)
                placementAreas[i].SetWarbandSlot(sortedWarband[i]);
        }

        private void FlipEnemySquadsSpriteX()
        {
            for (int i = 0; i < _enemyPlacementAreas.Count; i++)
            {
                if (_enemyPlacementAreas[i].Empty) continue;
                _enemyPlacementAreas[i].SquadIconSR.flipX = true;
            }
        }
    }
}
