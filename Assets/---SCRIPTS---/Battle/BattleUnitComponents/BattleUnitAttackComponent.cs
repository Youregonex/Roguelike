using UnityEngine;
using DG.Tweening;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitAttackComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        private const float MIN_ATTACK_SPEED = .01f;

        [CustomHeader("Debug")]
        [SerializeField] protected float _attackCooldownCurrent = 0f;
        [SerializeField] protected BattleUnitAnimationComponent _battleUnitAnimationComponent;
        [SerializeField] protected BattleUnitTargetComponent _battleUnitTargetComponent;

        protected BattleUnitStatsComponent _battleUnitStatsComponent;
        protected BattleUnitPerkComponent _battleUnitPerkComponent;

        private BattleUnitCore _currentTarget => _battleUnitTargetComponent.CurrentTarget;

        private Stat AttackRange;
        private Stat AttackDamage;
        private Stat AttackSpeed;
        private Stat KnockBackForce;

        protected bool _attackLocked = false;

        protected Tween _attackTween;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitAnimationComponent = _battleUnitCore.GetUnitComponent<BattleUnitAnimationComponent>();
            _battleUnitTargetComponent = _battleUnitCore.GetUnitComponent<BattleUnitTargetComponent>();
            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();
            _battleUnitPerkComponent = _battleUnitCore.GetUnitComponent<BattleUnitPerkComponent>();

            if (_battleUnitStatsComponent.IsInitialized)
                GetStats();
            else _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;


            float minDelay = .1f;
            float maxDelay = .3f;
            float randomDelay = UnityEngine.Random.Range(minDelay, maxDelay);
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;
            GetStats();
        }

        public virtual void Tick()
        {
            if (_attackCooldownCurrent > 0)
                _attackCooldownCurrent -= Time.deltaTime;
            else AttackDistanceCheck();
        }

        public void LockAttack() => _attackLocked = true;
        public void UnlockAttack() => _attackLocked = false;

        protected virtual void OnDestroy()
        {
            if (_attackTween != null)
                _attackTween.Kill();

            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
        }

        protected void AttackDistanceCheck()
        {
            if (_currentTarget == null) return;
            if (_attackCooldownCurrent > 0 || _attackLocked) return;

            if(Utilities.IsWithinRange(transform.position, _currentTarget.transform.position, AttackRange.CurrentValue))
                Attack(_currentTarget);
        }

        protected virtual void Attack(BattleUnitCore target)
        {
            DamageStruct damage = GenerateDamageStruct();
            _battleUnitPerkComponent.ApplyPerks(EPerkApplicationEvent.OnAttack, target, ref damage);

            ProccessAttack(damage, target);
            RefreshAttackCooldown();

            _battleUnitAnimationComponent.PlayAttackAnimation();
            PlayAttackAnimation(target.transform);
        }

        protected virtual void ProccessAttack(DamageStruct damageStruct, BattleUnitCore target)
        {
            _battleUnitCore.DealDamage(damageStruct, target, true);
        }

        protected void GetStats()
        {
            AttackRange = _battleUnitStatsComponent.GetStat(EStat.AttackRange);
            AttackDamage = _battleUnitStatsComponent.GetStat(EStat.AttackDamage);
            AttackSpeed = _battleUnitStatsComponent.GetStat(EStat.AttackSpeed);
            KnockBackForce = _battleUnitStatsComponent.GetStat(EStat.KnockBackForce);
        }

        protected virtual void PlayAttackAnimation(Transform targetTransform)
        {
            _attackTween?.Complete();

            if (targetTransform == null) return;

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
                    if (visualTransform == null || _battleUnitCore == null)
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
            return AttackDamage.CurrentValue;
        }

        protected virtual void RefreshAttackCooldown()
        {
            _attackCooldownCurrent = 1f / Mathf.Max(AttackSpeed.CurrentValue, MIN_ATTACK_SPEED);
        }

        protected virtual DamageStruct GenerateDamageStruct()
        {
            float attackDamage = CalculateDamage();
            return new(
                _battleUnitCore.UnitFaction,
                _battleUnitCore,
                transform.position,
                _battleUnitStatsComponent.AttackType,
                _battleUnitStatsComponent.DamageType,
                attackDamage,
                KnockBackForce.CurrentValue);
        }
    }
}