using UnityEngine;
using Yg.Pooling;
using Zenject;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitRangedAttackComponent : BattleUnitAttackComponent
    {
        private UltimatePooler _ultimatePooler;

        private BattleUnitRangedStatsComponent BattleUnitRangedStatsComponent => _battleUnitStatsComponent as BattleUnitRangedStatsComponent;

        [Inject]
        private void Construct(UltimatePooler ultimatePooler)
        {
            _ultimatePooler = ultimatePooler;
        }

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);
        }

        protected override void ProccessAttack(DamageStruct damageStruct, BattleUnitCore battleUnitCore)
        {
            Projectile projectile = _ultimatePooler.Dequeue(BattleUnitRangedStatsComponent.ProjectilePrefab);
            projectile.transform.position = transform.position;

            Vector2 direction = Utilities.GetDirectionVectorNormalized(transform.position, battleUnitCore.transform.position);
            Vector2 velocity = direction * BattleUnitRangedStatsComponent.ProjectileSpeed;

            projectile.transform.right = direction;

            if (!projectile.IsInitialized)
                projectile.Initialize(_ultimatePooler, BattleUnitRangedStatsComponent.ProjectilePrefab, damageStruct, velocity);
            else
                projectile.Setup(damageStruct, velocity);
        }
    }
}
