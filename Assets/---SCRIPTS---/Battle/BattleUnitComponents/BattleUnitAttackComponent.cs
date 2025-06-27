using UnityEngine;
using DG.Tweening;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitAttackComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] protected float _attackRange;
        [SerializeField] protected float _attackCooldownMin;
        [SerializeField] protected float _attackCooldownMax;
        [SerializeField] protected float _attackDamageMin;
        [SerializeField] protected float _attackDamageMax;
        [SerializeField] protected float _distanceCheckInterval;
        [SerializeField] protected float _knockBackForce;

        [CustomHeader("Debug")]
        [SerializeField] protected float _attackCooldownCurrent = 0f;
        [SerializeField] protected BattleUnitAnimationComponent _battleUnitAnimationComponent;
        [SerializeField] protected BattleUnitTargetComponent _battleUnitTargetComponent;

        protected Tween _attackTween;

        public float AttackRange => _attackRange;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitAnimationComponent = _battleUnitCore.GetUnitComponent<BattleUnitAnimationComponent>();
            _battleUnitTargetComponent = _battleUnitCore.GetUnitComponent<BattleUnitTargetComponent>();

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

        protected void OnDestroy()
        {
            if (_attackTween != null)
                _attackTween.Kill();
        }

        protected void AttackDistanceCheck()
        {
            if (_battleUnitTargetComponent.CurrentTarget is null || _attackCooldownCurrent > 0) return;
            if (Vector2.Distance(transform.position, _battleUnitTargetComponent.CurrentTarget.transform.position) <= _attackRange)
                Attack(_battleUnitTargetComponent.CurrentTarget);
        }

        protected virtual void Attack(BattleUnitCore battleUnitCore)
        {
            float attackDamage = CalculateDamage();
            DamageStruct damage = new(_battleUnitCore.UnitFaction, _battleUnitCore.transform.position, attackDamage, _knockBackForce);

            if (battleUnitCore.TryGetUnitComponent(out BattleUnitHealthComponent target))
                target.TakeDamage(damage);

            _attackCooldownCurrent = CalculateAttackCooldown();
            _battleUnitAnimationComponent.PlayAttackAnimation();
            PlayAttackAnimation(battleUnitCore.transform);
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
            return UnityEngine.Random.Range(_attackDamageMin, _attackDamageMax);
        }

        protected virtual float CalculateAttackCooldown()
        {
            return UnityEngine.Random.Range(_attackCooldownMin, _attackCooldownMax);
        }
    }
}