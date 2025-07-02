using UnityEngine;
using Yg.Battle.BattleUnits;

namespace Yg.Battle
{
    public class AOEDamageSpell : AOESpell
    {
        public AOEDamageSpell(AOEDamageSpellSO aoeDamageSpellSO) : base(aoeDamageSpellSO) {}

        public override bool TryCast(BattleUnitCore caster)
        {
            BattleUnitCore target = caster.GetUnitComponent<BattleUnitTargetComponent>().CurrentTarget;

            if (target is not null && Vector2.Distance(caster.transform.position, target.transform.position) <= ((AOESpellSO)_spellSO).Range)
            {
                Cast(caster);
                return true;
            }

            return false;
        }

        protected override void ApplySpellEffect(BattleUnitCore caster)
        {
            BattleUnitCore target = caster.GetUnitComponent<BattleUnitTargetComponent>().CurrentTarget;
            AOEDamageSpellSO aoeSpellSO = (AOEDamageSpellSO)_spellSO;

            GameObject spellVFX = GameObject.Instantiate(aoeSpellSO.SpellVFX, target.transform.position, Quaternion.identity);
            GameObject.Destroy(spellVFX, 2f);

            Collider2D[] hits = Physics2D.OverlapCircleAll(target.transform.position, aoeSpellSO.ImpactRadius);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out BattleUnitCore possibleTarget))
                {
                    if (possibleTarget.UnitFaction == caster.UnitFaction) continue;

                    DamageStruct damageStruct = new()
                    {
                        UnitFaction = caster.UnitFaction,
                        Origin = caster,
                        AttackType = EAttackType.Magic,
                        DamageType = aoeSpellSO.DamageType,
                        DamageAmount = aoeSpellSO.Damage,
                        KnockBackForce = 0f
                    };

                    caster.DealDamage(damageStruct, possibleTarget, true);
                }
            }
        }
    }
}
