using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitMovementComponent : BattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _proximityThreshold;

        private void Update()
        {
            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            if (_battleUnitCore.CurrentTarget is null || _battleUnitCore.CurrentTarget.transform is null) return;
            if (Vector2.Distance(transform.position, _battleUnitCore.CurrentTarget.transform.position) <= _proximityThreshold) return;

            transform.root.position = Vector2.MoveTowards(
                transform.position,
                _battleUnitCore.CurrentTarget.transform.position,
                _moveSpeed * Time.deltaTime);
        }
    }
}
