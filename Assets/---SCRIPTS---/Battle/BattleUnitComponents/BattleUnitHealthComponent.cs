using System;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitHealthComponent : BattleUnitComponent
    {
        public event Action<DamageStruct> OnDamageTaken;

        private BattleUnitPerkComponent _battleUnitPerkComponent;
        private BattleUnitStatsComponent _battleUnitStatsComponent;

        private Stat Health;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitPerkComponent = _battleUnitCore.GetUnitComponent<BattleUnitPerkComponent>();
            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();

            if (_battleUnitStatsComponent.IsInitialized)
                GetStats();
            else
                _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;
        }

        private void GetStats()
        {
            Health = _battleUnitStatsComponent.GetStat(EStat.Health);
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
            GetStats();
        }

        public void TakeDamage(DamageStruct damageStruct)
        {
            if (Health.CurrentValue <= 0) return;

            _battleUnitPerkComponent.ApplyPerks(EPerkApplicationEvent.OnDamageTaken, null, ref damageStruct);

            Health.DecreaseCurrentValue(damageStruct.DamageAmount, false);
            OnDamageTaken?.Invoke(damageStruct);

            if (Health.CurrentValue <= 0)
                Die();
        }

        private void Die()
        {
            _battleUnitCore.Death();
        }
    }
}