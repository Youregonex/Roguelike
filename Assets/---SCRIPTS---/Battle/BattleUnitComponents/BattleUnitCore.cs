using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Yg.Battle.BattleUnits
{
    [SelectionBase]
    public class BattleUnitCore : MonoBehaviour
    {
        public event Action<BattleUnitCore> OnDeath;
        public event Action<BattleUnitCore> OnTargetRemoval;

        private readonly HashSet<BattleUnitComponent> _battleUnitComponentList = new();
        private HashSet<BattleUnitCore> _targetList = new();
        private HashSet<ITickableBattleUnitComponent> _tickableComponentSet = new();

        [CustomHeader("Debug")]
        [SerializeField] private EUnitFaction _unitFaction;

        public EUnitFaction UnitFaction => _unitFaction;
        public IEnumerable<BattleUnitCore> TargetList => _targetList;

        public void Initialize(EUnitFaction unitFaction)
        {
            _unitFaction = unitFaction;

            GatherUnitComponents();
            InitializeUnitComponents();
        }

        private void Update()
        {
            foreach (var component in _tickableComponentSet)
                component.Tick();
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

            OnTargetRemoval?.Invoke(battleUnitCore);
        }

        private void GatherUnitComponents()
        {
            foreach (var component in GetComponentsInChildren<BattleUnitComponent>())
            {
                _battleUnitComponentList.Add(component);

                if (component is ITickableBattleUnitComponent)
                    _tickableComponentSet.Add(component as ITickableBattleUnitComponent);
            }
        }

        private void InitializeUnitComponents()
        {
            foreach (var component in _battleUnitComponentList)
                component.InitializeComponent(this);
        }
    }
}