using Yg.GameData.Units;
using System;
using System.Collections.Generic;
using Yg.GameData.Perks;
using System.Linq;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitStatsComponent : BattleUnitComponent
    {
        public event Action OnInitializationComplete;

        public EAttackType AttackType { get; private set; }
        public EDamageType DamageType { get; private set; }

        public List<Stat> UnitStatList { get; private set; } = new();
        public List<PerkSO> PerkSOList { get; private set; }
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
            for (int i = 0; i < unitDataSO.UnitStatDataList.Count; i++)
            {
                Stat stat = new(
                    unitDataSO.UnitStatDataList[i].StatType,
                    unitDataSO.UnitStatDataList[i].MaxValue,
                    unitDataSO.UnitStatDataList[i].IgnoreMaxValue);

                UnitStatList.Add(stat);
            }

            AttackType = unitDataSO.AttackType;
            DamageType = unitDataSO.DamageType;

            PerkSOList = new List<PerkSO>(unitDataSO.PerkSOList);
            SpellSOList = new List<SpellSO>(unitDataSO.SpellSOList);
        }

        public Stat GetStat(EStat statType)
        {
            return UnitStatList.Where(e => e.StatType == statType).FirstOrDefault();
        }

        public float GetMaxStatValue(EStat statType)
        {
            return UnitStatList.Where(e => e.StatType == statType).FirstOrDefault().MaxValue;
        }

        public float GetCurrentStatValue(EStat statType)
        {
            return UnitStatList.Where(e => e.StatType == statType).FirstOrDefault().CurrentValue;
        }

        public void IncreaseMaxStatValue(EStat statType, float amount, bool percentage)
        {
            if (amount < 0) return;

            UnitStatList.Where(e => e.StatType == statType).FirstOrDefault().IncreaseMaxValue(amount, percentage);
        }

        public void DecreaseMaxStatValue(EStat statType, float amount, bool percentage)
        {
            if (amount < 0) return;

            UnitStatList.Where(e => e.StatType == statType).FirstOrDefault().DecreaseMaxValue(amount, percentage);
        }

        public void IncreaseCurrentStatValue(EStat statType, float amount, bool percentage)
        {
            if (amount < 0) return;

            UnitStatList.Where(e => e.StatType == statType).FirstOrDefault().IncreaseCurrentValue(amount, percentage);
        }

        public void DecreaseCurrentStatValue(EStat statType, float amount, bool percentage)
        {
            if (amount < 0) return;

            UnitStatList.Where(e => e.StatType == statType).FirstOrDefault().DecreaseCurrentValue(amount, percentage);
        }
    }
}
