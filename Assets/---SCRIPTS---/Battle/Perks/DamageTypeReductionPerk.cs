using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public class DamageTypeReductionPerk : Perk
    {
        public DamageTypeReductionPerkSO DamageTypeReductionPerkSO { get; private set; }

        public DamageTypeReductionPerk(PerkSO perkSO) : base(perkSO)
        {
            if (!(perkSO is DamageTypeReductionPerkSO))
            {
                Debug.LogError("Wrong PerkSO!");
                return;
            }

            DamageTypeReductionPerkSO = perkSO as DamageTypeReductionPerkSO;
        }

        public override void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            if (damageStruct.DamageType == DamageTypeReductionPerkSO.DamageType)
                damageStruct.DamageAmount -= damageStruct.DamageAmount * DamageTypeReductionPerkSO.DamageReductionPercent;
        }
    }
}
