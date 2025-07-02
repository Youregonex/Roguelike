using UnityEngine;
using DG.Tweening;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitAttackComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] protected float _distanceCheckInterval;

        [CustomHeader("Debug")]
        [SerializeField] protected float _attackCooldownCurrent = 0f;
        [SerializeField] protected BattleUnitAnimationComponent _battleUnitAnimationComponent;
        [SerializeField] protected BattleUnitTargetComponent _battleUnitTargetComponent;

        protected BattleUnitStatsComponent _battleUnitStatsComponent;
        protected BattleUnitPerkComponent _battleUnitPerkComponent;

        private BattleUnitCore _currentTarget => _battleUnitTargetComponent.CurrentTarget;
        protected bool _attackLocked = false;

        protected Tween _attackTween;

        public float AttackRange => _battleUnitStatsComponent.AttackRange;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitAnimationComponent = _battleUnitCore.GetUnitComponent<BattleUnitAnimationComponent>();
            _battleUnitTargetComponent = _battleUnitCore.GetUnitComponent<BattleUnitTargetComponent>();
            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();
            _battleUnitPerkComponent = _battleUnitCore.GetUnitComponent<BattleUnitPerkComponent>();

            float minDelay = .1f;
            float maxDelay = .3f;
            float randomDelay = UnityEngine.Random.Range(minDelay, maxDelay);

            InvokeRepeating("AttackDistanceCheck", randomDelay, _distanceCheckInterval);
        }

        public virtual void Tick()
        {
            if (_attackCooldownCurrent > 0)
                _attackCooldownCurrent -= Time.deltaTime;
        }

        public void LockAttack() => _attackLocked = true;
        public void UnlockAttack() => _attackLocked = false;

        protected virtual void OnDestroy()
        {
            if (_attackTween != null)
                _attackTween.Kill();
        }

        protected void AttackDistanceCheck()
        {
            if (_currentTarget is null || _attackCooldownCurrent > 0 || _attackLocked) return;
            if (Vector2.Distance(transform.position, _currentTarget.transform.position) <= _battleUnitStatsComponent.AttackRange)
                Attack(_currentTarget);
        }

        protected virtual void Attack(BattleUnitCore target)
        {
            DamageStruct damage = GenerateDamageStruct();
            _battleUnitPerkComponent.ApplyPerks(EPerkApplicationEvent.OnAttack, _battleUnitCore, target, ref damage);

            ProccessAttack(damage, target);
            RefreshAttackCooldown();

            _battleUnitAnimationComponent.PlayAttackAnimation();
            PlayAttackAnimation(target.transform);
        }

        protected virtual void ProccessAttack(DamageStruct damageStruct, BattleUnitCore target)
        {
            _battleUnitCore.DealDamage(damageStruct, target, true);
        }

        protected virtual void PlayAttackAnimation(Transform targetTransform)
        {
            _attackTween?.Complete();

            if (targetTransform is null) return;

            float animationDuration = .25f;
            float moveAmount = .3f;

            Vector2 moveDirection = (targetTransform.position - transform.position).normalized;
            Vector2 targetPosition = (Vector2)transform.position + moveDirection * moveAmount;

            BattleUnitMovementComponent battleUnitMovementComponent = _battleUnitCore.GetUnitComponent<BattleUnitMovementComponent>();
            battleUnitMovementComponent.LockMovement();
            Transform visualTransform = _battleUnitAnimationComponent.transform;

            _attackTween = visualTransform
                .DOMove(targetPosition, animationDuration / 2f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    if (visualTransform is null || _battleUnitCore is null)
                    {
                        _attackTween = null;
                        return;
                    }

                    _attackTween = visualTransform
                    .DOMove(_battleUnitCore.transform.position, animationDuration / 2f)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        battleUnitMovementComponent.UnlockMovement();
                        _attackTween = null;
                    });
                });
        }

        protected virtual float CalculateDamage()
        {
            return UnityEngine.Random.Range(_battleUnitStatsComponent.AttackDamageMin, _battleUnitStatsComponent.AttackDamageMax);
        }

        protected virtual void RefreshAttackCooldown()
        {
            _attackCooldownCurrent =
                UnityEngine.Random.Range(_battleUnitStatsComponent.AttackCooldownMin, _battleUnitStatsComponent.AttackCooldownMax);
        }

        protected virtual DamageStruct GenerateDamageStruct()
        {
            float attackDamage = CalculateDamage();
            return new(
                _battleUnitCore.UnitFaction,
                _battleUnitCore,
                _battleUnitStatsComponent.AttackType,
                _battleUnitStatsComponent.DamageType,
                attackDamage,
                _battleUnitStatsComponent.KnockBackForce);
        }
    }
}