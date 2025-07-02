using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public class AttackTypeReductionPerk : Perk
    {
        public AttackTypeReductionPerkSO AttackTypeReductionPerkSO { get; private set; }

        public AttackTypeReductionPerk(PerkSO perkSO) : base(perkSO)
        {
            if(!(PerkSO is AttackTypeReductionPerkSO))
            {
                Debug.LogError("Wrong PerkSO!");
                return;
            }

            AttackTypeReductionPerkSO = perkSO as AttackTypeReductionPerkSO;
        }

        public override void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            if (damageStruct.AttackType == AttackTypeReductionPerkSO.DefenceFromAttackType)
                damageStruct.DamageAmount -= damageStruct.DamageAmount * AttackTypeReductionPerkSO.DamageReductionPercent;
        }
    }
}
