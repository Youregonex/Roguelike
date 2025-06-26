using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitCore : MonoBehaviour
    {
        public event Action<BattleUnitCore> OnDeath;

        [CustomHeader("Settings")]
        [SerializeField] private float _targetChangeInterval;

        private readonly HashSet<BattleUnitComponent> _battleUnitComponentList = new();
        private HashSet<BattleUnitCore> _targetList = new();

        [CustomHeader("Debug")]
        [SerializeField] private BattleUnitCore _currentTarget;
        [SerializeField] private EUnitFaction _unitFaction;

        public BattleUnitCore CurrentTarget => _currentTarget;
        public EUnitFaction UnitFaction => _unitFaction;

        public void Initialize(EUnitFaction unitFaction)
        {
            _unitFaction = unitFaction;

            GatherUnitComponents();
            InitializeUnitComponents();

            InvokeRepeating("UpdateCurrentTarget", 0f, _targetChangeInterval);
        }

        public T GetUnitComponent<T>() where T : BattleUnitComponent
        {
            return _battleUnitComponentList.OfType<T>().FirstOrDefault();
        }

        public bool TryGetUnitComponent<T>(out T unitComponent) where T : BattleUnitComponent
        {
            T component = _battleUnitComponentList.OfType<T>().FirstOrDefault();

            if (component is null)
            {
                unitComponent = null;
                return false;
            }

            unitComponent = component;
            return true;
        }

        public void Death()
        {
            OnDeath?.Invoke(this);
        }

        public void AssignTargets(List<BattleUnitCore> battleUnits)
        {
            _targetList = battleUnits.ToHashSet();
        }

        public void AddTarget(BattleUnitCore battleUnitCore)
        {
            if (!_targetList.Contains(battleUnitCore))
                _targetList.Add(battleUnitCore);
        }

        public void RemoveTarget(BattleUnitCore battleUnitCore)
        {
            if (_targetList.Contains(battleUnitCore))
                _targetList.Remove(battleUnitCore);

            if (_currentTarget == battleUnitCore)
                UpdateCurrentTarget();
        }

        private void UpdateCurrentTarget()
        {
            if (!_targetList.Any())
            {
                _currentTarget = null;
                return;
            }

            _currentTarget = _targetList
                .Aggregate((closest, next) =>
                    Vector2.Distance(transform.position, next.transform.position) <
                    Vector2.Distance(transform.position, closest.transform.position)
                    ? next : closest);
        }

        private void GatherUnitComponents()
        {
            foreach (var component in GetComponentsInChildren<BattleUnitComponent>())
                _battleUnitComponentList.Add(component);
        }

        private void InitializeUnitComponents()
        {
            foreach (var component in _battleUnitComponentList)
                component.InitializeComponent(this);
        }
    }

    public enum EUnitFaction
    {
        None,
        Player,
        Enemy
    }
}
