using UnityEngine;
using Yg.GameData.Units;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitRangedStatsComponent : BattleUnitStatsComponent
    {
        public Projectile ProjectilePrefab { get; private set; }
        public float ProjectileSpeed { get; private set; }

        protected override void SetupStats(UnitDataSO unitDataSO)
        {
            base.SetupStats(unitDataSO);

            if (unitDataSO is not RangedUnitDataSO)
            {
                Debug.LogError($"{_battleUnitCore.gameObject.name} | Couldn't setup stats, wrong data!");
                return;
            }

            RangedUnitDataSO rangedUnitDataSO = unitDataSO as RangedUnitDataSO;
            ProjectilePrefab = rangedUnitDataSO.ProjectilePrefab;
            ProjectileSpeed = rangedUnitDataSO.ProjectileSpeed;
        }
    }
}
