using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    [CreateAssetMenu(fileName = "Explosion", menuName = "Spells/Explosion")]
    public class ExplosionSO : SpellSO
    {
        public override Spell BuildSpell(BattleUnitCore caster, UltimatePooler ultimatePooler)
        {
            return new Explosion(this, caster, ultimatePooler);
        }

        protected override void Validate()
        {
            Description = $"Deals <b><color=#466B95>{Value}</color></b> <b><color=#466B95>{AffectionType}</color></b> damage in <b><color=#466B95>{ImpactRadius}m</color></b> radius.";
        }
    }

    public class Explosion : Spell
    {
        public Explosion(SpellSO spellSO, BattleUnitCore caster, UltimatePooler ultimatePooler) : base(spellSO, caster, ultimatePooler) { }

        public override bool TryCast(BattleUnitCore target)
        {
            if (
                target is not null &&
                Vector2.Distance(_caster.transform.position, target.transform.position) <= _spellSO.ActivationRange)
            {
                Cast(target);
                return true;
            }

            return false;
        }

        protected override void ApplySpellEffect(BattleUnitCore target)
        {
            _spellVFX.transform.position = target.transform.position;

            Collider2D[] hits = Physics2D.OverlapCircleAll(target.transform.position, _spellSO.ImpactRadius);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out BattleUnitCore possibleTarget))
                {
                    if (possibleTarget.UnitFaction == _caster.UnitFaction) continue;

                    DamageStruct damageStruct = new(
                        _caster.UnitFaction,
                        _caster,
                        target.transform.position,
                        EAttackType.Spell,
                        _spellSO.AffectionType,
                        _spellSO.Value,
                        _spellSO.KnockbackValue);

                    _caster.DealDamage(damageStruct, possibleTarget, true);

                    if (_spellSO.StatusEffectList is null) continue;

                    for (int i = 0; i < _spellSO.StatusEffectList.Count; i++)
                        if (possibleTarget.TryGetUnitComponent(out BattleUnitStatusEffectComponent battleUnitStatusEffectComponent))
                            battleUnitStatusEffectComponent.ApplyStatusEffect(_caster, _spellSO.StatusEffectList[i]);
                }
            }
        }
    }
}
