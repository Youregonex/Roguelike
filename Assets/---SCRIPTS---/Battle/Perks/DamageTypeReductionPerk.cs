using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    [CreateAssetMenu(fileName = "Perk", menuName = "Perks/DamageTypeReductionPerk")]
    public class DamageTypeReductionPerk : Perk
    {
        [field: SerializeField, Range(0f, 1f)] public float DamageReductionPercent { get; private set; }
        [field: SerializeField] public EDamageType DamageType { get; private set; }

        public override void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            if (damageStruct.DamageType == DamageType)
                damageStruct.DamageAmount -= damageStruct.DamageAmount * DamageReductionPercent;
        }

        protected override void Validate()
        {
            PerkDescription = $"Reduce <b><color=#466C96>{DamageType}</color></b> damage taken by <b><color=#466C96>{DamageReductionPercent * 100}%</color></b>.";
        }
    }
}
