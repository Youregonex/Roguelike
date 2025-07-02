using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    [CreateAssetMenu(fileName = "Perk", menuName = "Perks/DamageOnDamageDealt")]
    public class DamageOnDamageDealtPerk : Perk
    {
        [field: SerializeField] public EDamageType DamageType { get; protected set; }
        [field: SerializeField] public float Damage { get; protected set; }

        public override void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            if (target is null || applier is null) return;

            DamageStruct damage = new(applier.UnitFaction, applier, EAttackType.Magic, DamageType, Damage, 0f);
            applier.DealDamage(damage, target, false);
        }

        protected override void Validate()
        {
            PerkDescription = $"On attack deals additional <b><color=#466C96>{Damage}</color></b> <b><color=#466C96>{DamageType}</color></b> damage.";
        }
    }
}