using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitMovementComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _moveSpeed;

        private BattleUnitTargetComponent _battleUnitTargetComponent;
        private BattleUnitAttackComponent _battleUnitAttackComponent;
        private Rigidbody2D _rigidBody;

        private bool _movementLocked = false;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitTargetComponent = _battleUnitCore.GetUnitComponent<BattleUnitTargetComponent>();
            _battleUnitAttackComponent = _battleUnitCore.GetUnitComponent<BattleUnitAttackComponent>();
            _rigidBody = _battleUnitCore.GetComponent<Rigidbody2D>();
        }

        public void Tick()
        {
            MoveTowardsTarget();
        }

        public void LockMovement() => _movementLocked = true;
        public void UnlockMovement() => _movementLocked = false;

        private void MoveTowardsTarget()
        {
            if (_movementLocked)
            {
                StopMovement();
                return;
            }

            if (_battleUnitTargetComponent.CurrentTarget is null || _battleUnitTargetComponent.CurrentTarget.transform is null)
            {
                StopMovement();
                return;
            }

            if (Vector2.Distance(
                transform.position,
                _battleUnitTargetComponent.CurrentTarget.transform.position)
                <= _battleUnitAttackComponent.AttackRange)
            {
                StopMovement();
                return;
            }

            var velocityDirection = Utilities.GetDirectionVectorNormalized(transform.position, _battleUnitTargetComponent.CurrentTarget.transform.position);
            var resultVelocity = velocityDirection * _moveSpeed;
            _rigidBody.velocity = resultVelocity;
        }

        private void StopMovement()
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }
}
