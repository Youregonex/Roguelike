using Yg.GameData.Units;
using System;
using System.Collections.Generic;
using Yg.GameData.Perks;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitStatsComponent : BattleUnitComponent
    {
        public event Action OnInitializationComplete;

        public float MoveSpeed { get; private set; }
        public float MaxHealth { get; private set; }
        public EAttackType AttackType { get; private set; }
        public EDamageType DamageType { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldownMin { get; private set; }
        public float AttackCooldownMax { get; private set; }
        public float AttackDamageMin { get; private set; }
        public float AttackDamageMax { get; private set; }
        public float KnockBackForce { get; private set; }
        public List<PerkSO> PerkList { get; private set; }
        public List<SpellSO> SpellSOList { get; private set; }

        public bool IsInitialized { get; private set; } = false;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);
            UnitDataSO unitDataSO = _battleUnitCore.UnitData;

            SetupStats(unitDataSO);

            IsInitialized = true;
            OnInitializationComplete?.Invoke();
        }

        protected virtual void SetupStats(UnitDataSO unitDataSO)
        {
            MoveSpeed = unitDataSO.MoveSpeed;
            MaxHealth = unitDataSO.MaxHealth;
            AttackType = unitDataSO.AttackType;
            DamageType = unitDataSO.DamageType;
            AttackRange = unitDataSO.AttackRange;
            AttackCooldownMin = unitDataSO.AttackCooldownMin;
            AttackCooldownMax = unitDataSO.AttackCooldownMax;
            AttackDamageMin = unitDataSO.AttackDamageMin;
            AttackDamageMax = unitDataSO.AttackDamageMax;
            KnockBackForce = unitDataSO.KnockBackForce;
            PerkList = unitDataSO.PerkList;
            SpellSOList = unitDataSO.SpellSOList;
        }
    }
}
