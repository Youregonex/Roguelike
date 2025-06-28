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
        public EAttackType AttackType { get; private set; }
        public EDamageType DamageType { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldownMin { get; private set; }
        public float AttackCooldownMax { get; private set; }
        public float AttackDamageMin { get; private set; }
        public float AttackDamageMax { get; private set; }
        public float KnockBackForce { get; private set; }
        public List<Perk> PerkList { get; private set; }

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);
            UnitDataSO unitDataSO = _battleUnitCore.UnitData;

            SetupStats(unitDataSO);

            OnInitializationComplete?.Invoke();
        }

        protected virtual void SetupStats(UnitDataSO unitDataSO)
        {
            MoveSpeed = unitDataSO.MoveSpeed;
            AttackType = unitDataSO.AttackType;
            DamageType = unitDataSO.DamageType;
            AttackRange = unitDataSO.AttackRange;
            AttackCooldownMin = unitDataSO.AttackCooldownMin;
            AttackCooldownMax = unitDataSO.AttackCooldownMax;
            AttackDamageMin = unitDataSO.AttackDamageMin;
            AttackDamageMax = unitDataSO.AttackDamageMax;
            KnockBackForce = unitDataSO.KnockBackForce;
            PerkList = unitDataSO.PerkList;
        }
    }
}
