using UnityEngine;
using Yg.Pooling;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitRangedAttackComponent : BattleUnitAttackComponent
    {
        private BasePool<Projectile> _projectilePool;
        private BattleUnitRangedStatsComponent _battleUnitRangedStatsComponent => _battleUnitStatsComponent as BattleUnitRangedStatsComponent;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);
            if(_battleUnitRangedStatsComponent.ProjectilePrefab is null)
                _battleUnitRangedStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;
            else
                _projectilePool = new(_battleUnitRangedStatsComponent.ProjectilePrefab, transform);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach(Projectile projectile in _projectilePool.Pool)
            {
                projectile.OnProjectileDestruction -= Projectile_OnProjectileDestruction;
                projectile.DeactivatePooling();
            }
        }

        protected override void ProccessAttack(DamageStruct damageStruct, BattleUnitCore battleUnitCore)
        {
            Projectile projectile = _projectilePool.Dequeue();
            projectile.OnProjectileDestruction += Projectile_OnProjectileDestruction;
            projectile.transform.position = transform.position;

            Vector2 direction = Utilities.GetDirectionVectorNormalized(transform.position, battleUnitCore.transform.position);
            Vector2 velocity = direction * _battleUnitRangedStatsComponent.ProjectileSpeed;

            projectile.transform.right = direction;
            projectile.Initialize(damageStruct, velocity);
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitRangedStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
            _projectilePool = new(_battleUnitRangedStatsComponent.ProjectilePrefab, transform);
        }

        private void Projectile_OnProjectileDestruction(Projectile projectile)
        {
            projectile.OnProjectileDestruction -= Projectile_OnProjectileDestruction;
            _projectilePool.Enqueue(projectile);
        }
    }
}
