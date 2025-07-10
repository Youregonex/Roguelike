using System.Collections;
using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    [CreateAssetMenu(fileName = "LightningShield", menuName = "Spells/LightningShield")]
    public class LightningShieldSO : SpellSO
    {
        [field: SerializeField] public float DamageInterval { get; private set; }

        public override Spell BuildSpell(BattleUnitCore caster, UltimatePooler pooler)
        {
            return new LightningShield(this, caster, pooler);
        }

        protected override void Validate()
        {
            Description = $"Creates lightning shield around caster which deals <b><color=#466B95>{Value} {AffectionType}</color></b> damage in <b><color=#466B95>{ImpactRadius}m</color></b> radius every <b><color=#466B95>{DamageInterval}s</color></b> for <b><color=#466B95>{Duration}s</color></b>.";
        }
    }

    public class LightningShield : Spell
    {
        private float _currentDuration;
        private DamageStruct _damageStruct;
        private float _damageInterval;

        private readonly Collider2D[] _targetBuffer = new Collider2D[50];
        private readonly Collider2D[] _tryCastBuffer = new Collider2D[1];

        public LightningShield(SpellSO spellSO, BattleUnitCore caster, UltimatePooler ultimatePooler) : base(spellSO, caster, ultimatePooler)
        {
            _damageStruct = new DamageStruct(
                _caster.UnitFaction,
                _caster,
                _caster.transform.position,
                EAttackType.Spell,
                _spellSO.AffectionType,
                _spellSO.Value,
                _spellSO.KnockbackValue);

            _damageInterval = (_spellSO as LightningShieldSO).DamageInterval;
        }

        public override bool TryCast(BattleUnitCore target)
        {
            if(Physics2D.OverlapCircleNonAlloc(_caster.transform.position, _spellSO.ActivationRange, _tryCastBuffer) > 0 &&
               _tryCastBuffer[0].TryGetComponent(out BattleUnitCore battleUnitCore) &&
               battleUnitCore.UnitFaction != _caster.UnitFaction)
                    Cast(target);
            
            return false;
        }

        protected override void StartCast(BattleUnitCore target)
        {
            base.StartCast(target);
            _currentDuration = _spellSO.Duration;
        }

        protected override void ApplySpellEffect(BattleUnitCore target)
        {
            _spellVFX.transform.SetParent(_caster.transform);
            _spellVFX.transform.localPosition = Vector2.zero;
            _caster.StartCoroutine(DamageInIntervalsCoroutine());
        }

        private IEnumerator DamageInIntervalsCoroutine()
        {
            int hitCount;
            while (_currentDuration > 0)
            {
                _currentDuration -= _damageInterval;

                hitCount = Physics2D.OverlapCircleNonAlloc(_caster.transform.position, _spellSO.ImpactRadius, _targetBuffer);
                for (int i = 0; i < hitCount; i++)
                {
                    var collider = _targetBuffer[i];
                    if (collider == null) continue;

                    if (collider.transform.TryGetComponent(out BattleUnitCore possibleTarget))
                    {
                        if (possibleTarget == null || possibleTarget.UnitFaction == _caster.UnitFaction)
                            continue;

                        _caster.DealDamage(_damageStruct, possibleTarget, true);
                    }
                }

                yield return new WaitForSeconds(_damageInterval);
            }
        }
    }
}