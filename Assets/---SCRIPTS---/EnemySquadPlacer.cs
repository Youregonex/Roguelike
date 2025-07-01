using System.Collections.Generic;
using UnityEngine;
using Yg.GameData;
using Zenject;
using System.Linq;
using Yg.Character;

namespace Yg.Battle
{
    public class EnemySquadPlacer : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private List<SquadPlacementArea> _enemySquadPlacementAreaList;

        private PersistentData _persistentData;

        private int _lastPlacementIndex;

        [Inject]
        private void Construct(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void Initialize()
        {
            PlaceEnemySquads();
        }

        private void PlaceEnemySquads()
        {
            _enemySquadPlacementAreaList = _enemySquadPlacementAreaList.OrderBy(e => e.transform.position.x).ToList();

            List<WarbandSlot> enemyWarband = _persistentData.BattleTransitionData.EnemyWarband;

            List<WarbandSlot> enemyMeleeSquads = enemyWarband.Where(e => e.UnitData.AttackType == EAttackType.Melee).ToList();
            List<WarbandSlot> enemyRangeSquads = enemyWarband.Where(e => e.UnitData.AttackType == EAttackType.Ranged).ToList();

            for (int i = 0; i < enemyMeleeSquads.Count; i++)
            {
                if (i < _enemySquadPlacementAreaList.Count)
                {
                    _enemySquadPlacementAreaList[i].SetWarbandSlot(enemyMeleeSquads[i]);
                    _lastPlacementIndex = i + 1;
                }
                else
                {
                    Debug.LogError("Not enough placement areas for melee squads!");
                    return;
                }
            }

            for (int i = 0; i < enemyRangeSquads.Count; i++)
            {
                int placementIndex = i + _lastPlacementIndex;
                if (placementIndex < _enemySquadPlacementAreaList.Count)
                {
                    _enemySquadPlacementAreaList[placementIndex].SetWarbandSlot(enemyRangeSquads[i]);
                }
                else
                {
                    Debug.LogError("Not enough placement areas for ranged squads!");
                    return;
                }
            }
        }
    }
}
