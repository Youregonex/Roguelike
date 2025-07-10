using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Yg.GameData.Units;

namespace Yg.Battle.BattleUnits
{
    [SelectionBase]
    public class BattleUnitCore : MonoBehaviour
    {
        public event Action<BattleUnitCore> OnDeath;
        public event Action<BattleUnitCore> OnTargetRemoval;
        public event Action<BattleUnitCore, float> OnDamageDealt;

        [CustomHeader("Settings")]
        [SerializeField] private UnitDataSO _unitData;

        private readonly HashSet<BattleUnitComponent> _battleUnitComponentList = new();
        private HashSet<BattleUnitCore> _targetList = new();
        private HashSet<ITickableBattleUnitComponent> _tickableComponentSet = new();

        [CustomHeader("Debug")]
        [SerializeField] private EUnitFaction _unitFaction;

        public UnitDataSO UnitData => _unitData;
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

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void DealDamage(DamageStruct damageStruct, BattleUnitCore target, bool applyPerks)
        {
            if (target is null) return;

            if (applyPerks && target.TryGetUnitComponent(out BattleUnitPerkComponent perkComponent))
                perkComponent.ApplyPerks(EPerkApplicationEvent.OnDamageDealt, target, ref damageStruct);

            if (target.TryGetUnitComponent(out BattleUnitHealthComponent targetHealthComponent))
            {
                targetHealthComponent.TakeDamage(damageStruct);
                OnDamageDealt?.Invoke(this, damageStruct.DamageAmount);
            }
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
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
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