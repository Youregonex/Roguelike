using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitMovementComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        private BattleUnitTargetComponent _battleUnitTargetComponent;
        private BattleUnitStatsComponent _battleUnitStatsComponent;
        private Rigidbody2D _rigidBody;

        private Stat MoveSpeed;
        private Stat AttackRange;
        private Stat KnockbackResistance;

        private Vector2 _knockbackVelocity;
        private bool _movementLocked = false;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitTargetComponent = _battleUnitCore.GetUnitComponent<BattleUnitTargetComponent>();
            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();

            if (_battleUnitStatsComponent.IsInitialized) GetStats();
            else _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;

            _rigidBody = _battleUnitCore.GetComponent<Rigidbody2D>();
        }

        public void Tick()
        {
            _knockbackVelocity = Vector2.Lerp(_knockbackVelocity, Vector2.zero, Time.deltaTime * KnockbackResistance.CurrentValue);

            Vector2 finalVelocity = _knockbackVelocity;

            if (!_movementLocked && _battleUnitTargetComponent.CurrentTarget != null)
            {
                if (!Utilities.IsWithinRange(transform.position, _battleUnitTargetComponent.CurrentTarget.transform.position, AttackRange.CurrentValue))
                {
                    var moveDir = Utilities.GetDirectionVectorNormalized(
                        transform.position,
                        _battleUnitTargetComponent.CurrentTarget.transform.position);

                    finalVelocity += moveDir * MoveSpeed.CurrentValue;
                }
            }

            _rigidBody.velocity = finalVelocity;
        }

        protected virtual void OnDestroy()
        {
            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
        }

        public void AddKnockback(Vector2 force)
        {
            _knockbackVelocity += force;
        }

        public void LockMovement() => _movementLocked = true;
        public void UnlockMovement() => _movementLocked = false;

        private void GetStats()
        {
            AttackRange = _battleUnitStatsComponent.GetStat(EStat.AttackRange);
            MoveSpeed = _battleUnitStatsComponent.GetStat(EStat.MoveSpeed);
            KnockbackResistance = _battleUnitStatsComponent.GetStat(EStat.KnockBackResistance);
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
            GetStats();
        }
    }
}