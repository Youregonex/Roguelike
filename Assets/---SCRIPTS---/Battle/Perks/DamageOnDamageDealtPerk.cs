using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public class DamageOnDamageDealtPerk : Perk
    {
        public DamageOnDamageDealtPerkSO DamageOnDamageDealtPerkSO { get; private set; }

        public DamageOnDamageDealtPerk(PerkSO perkSO) : base(perkSO)
        {
            if (!(PerkDataSO is DamageOnDamageDealtPerkSO))
            {
                Debug.LogError("Wrong PerkSO!");
                return;
            }

            DamageOnDamageDealtPerkSO = perkSO as DamageOnDamageDealtPerkSO;
        }

        public override void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            if (target == null || applier == null) return;

            DamageStruct damage = new(
                applier.UnitFaction,
                applier,
                applier.transform.position,
                EAttackType.Spell,
                DamageOnDamageDealtPerkSO.DamageType,
                DamageOnDamageDealtPerkSO.Damage,
                0f);

            applier.DealDamage(damage, target, false);
        }
    }
}
