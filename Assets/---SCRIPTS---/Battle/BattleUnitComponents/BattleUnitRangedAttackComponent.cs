using UnityEngine;
using Yg.Pooling;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitRangedAttackComponent : BattleUnitAttackComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private float _projectileSpeed;

        private BasePool<Projectile> _projectilePool;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);
            _projectilePool = new(_projectilePrefab, transform);
        }

        protected override void Attack(BattleUnitCore battleUnitCore)
        {
            Projectile projectile = _projectilePool.Dequeue();
            projectile.OnProjectileDestruction += Projectile_OnProjectileDestruction;
            projectile.transform.position = transform.position;

            Vector2 direction = Utilities.GetDirectionVectorNormalized(transform.position, battleUnitCore.transform.position);
            Vector2 velocity = direction * _projectileSpeed;
            DamageStruct damageStruct = new(_battleUnitCore.UnitFaction, transform.position, CalculateDamage(), _knockBackForce);

            projectile.transform.right = direction;
            projectile.Initialize(damageStruct, velocity);

            _attackCooldownCurrent = CalculateAttackCooldown();
            _battleUnitAnimationComponent.PlayAttackAnimation();
        }

        private void Projectile_OnProjectileDestruction(Projectile projectile)
        {
            projectile.OnProjectileDestruction -= Projectile_OnProjectileDestruction;
            _projectilePool.Enqueue(projectile);
        }
    }
}
