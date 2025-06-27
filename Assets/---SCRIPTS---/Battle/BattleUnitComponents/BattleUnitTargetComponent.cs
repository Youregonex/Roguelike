using System.Linq;
using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitTargetComponent : BattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _targetSelectionInterval;

        private BattleUnitCore _currentTarget;

        public BattleUnitCore CurrentTarget => _currentTarget;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitCore.OnTargetRemoval += BattleUnitCore_OnTargetRemoval;

            float minDelay = .1f;
            float maxDelay = .3f;
            float randomDelay = UnityEngine.Random.Range(minDelay, maxDelay);
            InvokeRepeating("UpdateCurrentTarget", randomDelay, _targetSelectionInterval);
        }

        private void OnDestroy()
        {
            _battleUnitCore.OnTargetRemoval -= BattleUnitCore_OnTargetRemoval;
        }

        private void BattleUnitCore_OnTargetRemoval(BattleUnitCore battleUnitCore)
        {
            if (_currentTarget == battleUnitCore)
                UpdateCurrentTarget();
        }

        private void UpdateCurrentTarget()
        {
            if (!_battleUnitCore.TargetList.Any())
            {
                _currentTarget = null;
                return;
            }

            _currentTarget = _battleUnitCore.TargetList
                .Aggregate((closest, next) =>
                    Vector2.Distance(transform.position, next.transform.position) <
                    Vector2.Distance(transform.position, closest.transform.position)
                    ? next : closest);
        }
    }
}
