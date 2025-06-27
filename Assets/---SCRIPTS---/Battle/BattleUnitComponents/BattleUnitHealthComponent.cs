using UnityEngine;
using System;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitHealthComponent : BattleUnitComponent
    {
        public event Action<DamageStruct> OnDamageTaken;

        [CustomHeader("Settings")]
        [SerializeField] private float _maxHealth;

        [CustomHeader("Debug")]
        [SerializeField] private float _currentHealth;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(DamageStruct damageStruct)
        {
            if (_currentHealth <= 0) return;
            
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