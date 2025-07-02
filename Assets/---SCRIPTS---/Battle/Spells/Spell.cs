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

        protected virtual void StartCast(BattleUnitCore caster)
        {
            if(caster.TryGetUnitComponent(out BattleUnitAttackComponent battleUnitAttackComponent))
                battleUnitAttackComponent.LockAttack();

            if (caster.TryGetUnitComponent(out BattleUnitMovementComponent battleUnitMovementComponent))
                battleUnitMovementComponent.LockMovement();
        }

        protected abstract void ApplySpellEffect(BattleUnitCore caster);

        protected virtual void StopCast(BattleUnitCore caster)
        {
            if (caster.TryGetUnitComponent(out BattleUnitAttackComponent battleUnitAttackComponent))
                battleUnitAttackComponent.UnlockAttack();

            if (caster.TryGetUnitComponent(out BattleUnitMovementComponent battleUnitMovementComponent))
                battleUnitMovementComponent.UnlockMovement();

            ApplyCooldown();
        }

        protected virtual void Cast(BattleUnitCore caster)
        {
            StartCast(caster);
            ApplySpellEffect(caster);
            StopCast(caster);
        }

        private void ApplyCooldown() => _currentCooldown = _spellSO.Cooldown;

    }
}