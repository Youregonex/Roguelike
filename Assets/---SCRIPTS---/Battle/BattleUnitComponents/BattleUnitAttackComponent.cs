using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitAttackComponent : BattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackCooldownMin;
        [SerializeField] private float _attackCooldownMax;
        [SerializeField] private float _attackDamageMin;
        [SerializeField] private float _attackDamageMax;
        [SerializeField] private float _distanceCheckInterval;

        private float _attackCooldownCurrent = 0f;
        private BattleUnitAnimationComponent _battleUnitAnimationComponent;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitAnimationComponent = _battleUnitCore.GetUnitComponent<BattleUnitAnimationComponent>();
            InvokeRepeating("AttackDistanceCheck", 0f, _distanceCheckInterval);
        }

        private void Update()
        {
            if (_attackCooldownCurrent > 0)
                _attackCooldownCurrent -= Time.deltaTime;
        }

        private void AttackDistanceCheck()
        {
            if (_battleUnitCore.CurrentTarget is null || _attackCooldownCurrent > 0) return;
            if (Vector2.Distance(transform.position, _battleUnitCore.CurrentTarget.transform.position) <= _attackRange)
                Attack(_battleUnitCore.CurrentTarget);
        }

        private void Attack(BattleUnitCore battleUnitCore)
        {
            float attackDamage = UnityEngine.Random.Range(_attackDamageMin, _attackDamageMax);
            DamageStruct damage = new(_battleUnitCore.transform, attackDamage);

            if (battleUnitCore.TryGetUnitComponent(out BattleUnitHealthComponent target))
                target.TakeDamage(damage);

            _attackCooldownCurrent = UnityEngine.Random.Range(_attackCooldownMin, _attackCooldownMax);
            _battleUnitAnimationComponent.PlayAttackAnimation();
        }
    }
}
