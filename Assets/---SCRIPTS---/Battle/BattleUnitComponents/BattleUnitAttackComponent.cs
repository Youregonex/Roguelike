using UnityEngine;
using DG.Tweening;

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
        [SerializeField] private float _knockBackForce;

        [CustomHeader("Debug")]
        [SerializeField] private float _attackCooldownCurrent = 0f;
        [SerializeField] private BattleUnitAnimationComponent _battleUnitAnimationComponent;
        [SerializeField] private BattleUnitTargetComponent _battleUnitTargetComponent;

        private Tween _attackTween;

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

        private void Update()
        {
            if (_attackCooldownCurrent > 0)
                _attackCooldownCurrent -= Time.deltaTime;
        }

        private void OnDestroy()
        {
            if (_attackTween != null)
                _attackTween.Kill();
        }

        private void AttackDistanceCheck()
        {
            if (_battleUnitTargetComponent.CurrentTarget is null || _attackCooldownCurrent > 0) return;
            if (Vector2.Distance(transform.position, _battleUnitTargetComponent.CurrentTarget.transform.position) <= _attackRange)
                Attack(_battleUnitTargetComponent.CurrentTarget);
        }

        private void Attack(BattleUnitCore battleUnitCore)
        {
            float attackDamage = UnityEngine.Random.Range(_attackDamageMin, _attackDamageMax);
            DamageStruct damage = new(_battleUnitCore.transform, attackDamage, _knockBackForce);

            if (battleUnitCore.TryGetUnitComponent(out BattleUnitHealthComponent target))
                target.TakeDamage(damage);

            _attackCooldownCurrent = UnityEngine.Random.Range(_attackCooldownMin, _attackCooldownMax);
            //_battleUnitAnimationComponent.PlayAttackAnimation();
            PlayAttackAnimation(battleUnitCore.transform);
        }

        private void PlayAttackAnimation(Transform targetTransform)
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
    }
}