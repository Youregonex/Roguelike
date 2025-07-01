using UnityEngine;
using Yg.Battle;

namespace Yg.GameData.Perks
{
    [CreateAssetMenu(fileName = "Perk", menuName = "Perks/DamageReductionPerk")]
    public class AttackTypeReductionPerk : Perk
    {
        [field: SerializeField, Range(0f, 1f)] public float DamageReductionPercent { get; private set; }
        [field: SerializeField] public EAttackType DefenceFromAttackType { get; private set; }

        public override void ApplyPerk(ref DamageStruct damageStruct)
        {
            if (damageStruct.AttackType == DefenceFromAttackType)
                damageStruct.DamageAmount -= damageStruct.DamageAmount * DamageReductionPercent;
        }

        private void OnValidate()
        {
            PerkDescription = $"Reduce <b><color=#466C96>{DefenceFromAttackType}</color></b> damage taken by <b><color=#466C96>{DamageReductionPercent * 100}%</color></b>.";
        }
    }
}
