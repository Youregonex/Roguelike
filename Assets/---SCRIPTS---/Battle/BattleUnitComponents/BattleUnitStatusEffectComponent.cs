using System.Collections.Generic;
using System.Linq;
using Yg.Pooling;
using Zenject;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitStatusEffectComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        private UltimatePooler _pooler;

        private readonly HashSet<StatusEffect> _statusEffectSet = new();
        private readonly HashSet<StatusEffect> _endedStatusEffectSet = new();

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

        private void BattleUnitCore_OnDeath(BattleUnitCore obj)
        {
            foreach (var statusEffect in _statusEffectSet)
                statusEffect.Remove();

            foreach (var statusEffect in _endedStatusEffectSet)
                statusEffect.Remove();
        }

        public void ApplyStatusEffect(BattleUnitCore applier, StatusEffectSO statusEffectSO)
        {
            if(_statusEffectSet.Where(e => e.StatusEffectSO.Name == statusEffectSO.Name).Any())
                return;
            
            StatusEffect statusEffect = statusEffectSO.BuildStatusEffect(applier, _battleUnitCore, _pooler);
            _statusEffectSet.Add(statusEffect);
            statusEffect.Initialize();
        }

        public void RemoveStatusEffect(StatusEffectSO statusEffectSO)
        {
            StatusEffect statusEffect = _statusEffectSet.Where(e => e.StatusEffectSO == statusEffectSO).FirstOrDefault();
            RemoveStatusEffect(statusEffect);
        }

        public void RemoveStatusEffect(StatusEffect statusEffect)
        {
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
