using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitTargetComponent : BattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _targetSelectionInterval = .5f;
        [SerializeField] private float _targetCheckRadius = 5f;
        [SerializeField] private LayerMask _unitLayerMask;

        [CustomHeader("Debug")]
        [SerializeField] private BattleUnitCore _currentTarget;

        private UnitRegistry _unitRegistry;
        private readonly List<BattleUnitCore> _closeCheckAllieList = new();
        private readonly List<BattleUnitCore> _closeCheckEnemyList = new();

        private readonly Collider2D[] _unitCheckBuffer = new Collider2D[50];

        public BattleUnitCore CurrentTarget => _currentTarget;

        [Inject]
        private void Construct(UnitRegistry unitRegistry)
        {
            _unitRegistry = unitRegistry;
        }

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitCore.OnTargetRemoval += BattleUnitCore_OnTargetRemoval;

            float minDelay = .1f;
            float maxDelay = .5f;
            float randomDelay = UnityEngine.Random.Range(minDelay, maxDelay);
            InvokeRepeating(nameof(UpdateCurrentTarget), randomDelay, _targetSelectionInterval);
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
            CheckSurroundingUnits();
            _currentTarget = GetClosestEnemy(_closeCheckEnemyList);

            if (_currentTarget != null) return;

            _currentTarget = GetClosestEnemy(_unitRegistry.GetEnemyList(_battleUnitCore.UnitFaction));
        }

        private BattleUnitCore GetClosestEnemy(IReadOnlyList<BattleUnitCore> unitList)
        {
            float closestSqrDistance = float.MaxValue;
            float sqrDistance;
            BattleUnitCore closestEnemy = null;

            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i] == null) continue;

                sqrDistance = Utilities.GetSqrDistance(transform.position, unitList[i].transform.position);

                if (sqrDistance >= closestSqrDistance) continue;

                closestSqrDistance = sqrDistance;
                closestEnemy = unitList[i];
            }

            return closestEnemy;
        }

        private void CheckSurroundingUnits()
        {
            _closeCheckAllieList.Clear();
            _closeCheckEnemyList.Clear();
            int hits = Physics2D.OverlapCircleNonAlloc(transform.position, _targetCheckRadius, _unitCheckBuffer, _unitLayerMask);

            if (hits == 0) return;

            for (int i = 0; i < hits; i++)
            {
                if (!_unitCheckBuffer[i].TryGetComponent(out BattleUnitCore unit)) continue;

                if(unit.UnitFaction == _battleUnitCore.UnitFaction)
                    _closeCheckAllieList.Add(unit);
                else
                    _closeCheckEnemyList.Add(unit);
            }
        }
    }
}