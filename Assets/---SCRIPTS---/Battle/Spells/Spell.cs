using UnityEngine;
using Yg.Battle.BattleUnits;

namespace Yg.Battle
{
    public abstract class Spell
    {
        protected SpellSO _spellSO;
        protected float _currentCooldown = 0f;

        public bool OnCooldown => _currentCooldown > 0;

        public Spell(SpellSO spellSO)
        {
            _spellSO = spellSO;
        }

        public void CooldownTick()
        {
            if (_currentCooldown > 0)
                _currentCooldown -= Time.deltaTime;
        }

        public abstract bool TryCast(BattleUnitCore caster);
        protected abstract void ApplySpellEffect(BattleUnitCore caster);

        protected virtual void Cast(BattleUnitCore caster)
        {
            ApplySpellEffect(caster);
            ApplyCooldown();
        }

        private void ApplyCooldown() => _currentCooldown = _spellSO.Cooldown;

    }
}