using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitMovementComponent : BattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _proximityThreshold;

        private BattleUnitTargetComponent _battleUnitTargetComponent;
        private Rigidbody2D _rigidBody;

        private bool _movementLocked = false;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitTargetComponent = _battleUnitCore.GetUnitComponent<BattleUnitTargetComponent>();
            _rigidBody = _battleUnitCore.GetComponent<Rigidbody2D>();
        }

        private void Update()
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

            if (Vector2.Distance(transform.position, _battleUnitTargetComponent.CurrentTarget.transform.position) <= _proximityThreshold)
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
