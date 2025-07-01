using UnityEngine;
using System;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitHealthComponent : BattleUnitComponent
    {
        public event Action<DamageStruct> OnDamageTaken;

        [CustomHeader("Debug")]
        [SerializeField] private float _currentHealth;

        private BattleUnitPerkComponent _battleUnitPerkComponent;
        private BattleUnitStatsComponent _battleUnitStatsComponent;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitPerkComponent = _battleUnitCore.GetUnitComponent<BattleUnitPerkComponent>();
            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();

            if(_battleUnitStatsComponent.MaxHealth == 0)
                _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;
            else
                _currentHealth = _battleUnitStatsComponent.MaxHealth;
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
            _currentHealth = _battleUnitStatsComponent.MaxHealth;
        }

        public void TakeDamage(DamageStruct damageStruct)
        {
            if (_currentHealth <= 0) return;

            _battleUnitPerkComponent.ApplyPerks(EPerkApplicationEvent.OnDamageTaken, ref damageStruct);

            _currentHealth -= damageStruct.DamageAmount;
            OnDamageTaken?.Invoke(damageStruct);

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
        }

        private void Die()
        {
            _battleUnitCore.Death();
        }
    }
}