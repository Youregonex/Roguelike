using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.Pooling;
using Zenject;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitStatusEffectComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        private UltimatePooler _pooler;

        private readonly HashSet<StatusEffect> _statusEffectSet = new();
        private readonly HashSet<StatusEffect> _endedStatusEffectSet = new();

        [SerializeField] private List<EStatusEffectTag> _appliedStatusTagList = new();

        [Inject]
        private void Construct(UltimatePooler pooler)
        {
            _pooler = pooler;
        }

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitCore.OnDeath += BattleUnitCore_OnDeath;
        }

        private void BattleUnitCore_OnDeath(BattleUnitCore battleUnitCore)
        {
            foreach (var statusEffect in _statusEffectSet.ToList())
                RemoveStatusEffect(statusEffect);

            foreach (var statusEffect in _endedStatusEffectSet.ToList())
                RemoveStatusEffect(statusEffect);
        }

        public bool AffectedBy(EStatusEffectTag statusEffectTag)
        {
            if (_appliedStatusTagList.Contains(statusEffectTag)) return true;

            return false;
        }

        public void ApplyStatusEffect(BattleUnitCore applier, StatusEffectSO statusEffectSO)
        {            
            StatusEffect statusEffect = statusEffectSO.BuildStatusEffect(applier, _battleUnitCore, _pooler);
            _statusEffectSet.Add(statusEffect);
            statusEffect.Initialize();

            _appliedStatusTagList.Add(statusEffectSO.StatusEffectTag);
        }

        public void RemoveStatusEffect(StatusEffectSO statusEffectSO)
        {
            StatusEffect statusEffect = _statusEffectSet.Where(e => e.StatusEffectSO == statusEffectSO).FirstOrDefault();
            RemoveStatusEffect(statusEffect);
        }

        public void RemoveStatusEffect(StatusEffect statusEffect)
        {
            _appliedStatusTagList.Remove(statusEffect.StatusEffectSO.StatusEffectTag);
            statusEffect.Remove();
            _statusEffectSet.Remove(statusEffect);
        }

        public void Tick()
        {
            foreach (var statusEffect in _statusEffectSet)
            {
                if (statusEffect.StatusEffectSO.Tickable)
                    statusEffect.Tick();

                statusEffect.DurationTick();

                if (statusEffect.Expired)
                    _endedStatusEffectSet.Add(statusEffect);
            }

            foreach (var endedStatusEffect in _endedStatusEffectSet)
                RemoveStatusEffect(endedStatusEffect);

            _endedStatusEffectSet.Clear();
        }
    }
}
