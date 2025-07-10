using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    public abstract class Spell
    {
        protected SpellSO _spellSO;
        protected UltimatePooler _pooler;
        protected BattleUnitCore _caster;
        protected SpellVFXGameObject _spellVFX;
        protected float _currentCooldown = 0f;

        public bool OnCooldown => _currentCooldown > 0;

        public Spell(SpellSO spellSO, BattleUnitCore caster, UltimatePooler ultimatePooler)
        {
            _caster = caster;
            _pooler = ultimatePooler;
            _spellSO = spellSO;
        }

        public void CooldownTick()
        {
            if (_currentCooldown > 0)
                _currentCooldown -= Time.deltaTime;
        }

        public abstract bool TryCast(BattleUnitCore target);

        protected virtual void StartCast(BattleUnitCore target)
        {
            if(_caster.TryGetUnitComponent(out BattleUnitAttackComponent attackComponent))
                attackComponent.LockAttack();

            if (_caster.TryGetUnitComponent(out BattleUnitMovementComponent movementComponent))
                movementComponent.LockMovement();

            if (_caster.TryGetUnitComponent(out BattleUnitPerkComponent perkComponent))
                perkComponent.ApplyPerks(EPerkApplicationEvent.OnSpellCast, target);

                InitializeVFX();
        }

        protected abstract void ApplySpellEffect(BattleUnitCore target);

        protected virtual void StopCast(BattleUnitCore target)
        {
            if (_caster.TryGetUnitComponent(out BattleUnitAttackComponent battleUnitAttackComponent))
                battleUnitAttackComponent.UnlockAttack();

            if (_caster.TryGetUnitComponent(out BattleUnitMovementComponent battleUnitMovementComponent))
                battleUnitMovementComponent.UnlockMovement();

            ApplyCooldown();
        }

        protected virtual void Cast(BattleUnitCore target)
        {
            StartCast(target);
            ApplySpellEffect(target);
            StopCast(target);
        }

        protected virtual void InitializeVFX()
        {
            _spellVFX = _pooler.Dequeue(_spellSO.VFX);

            if (!_spellVFX.IsInitialized) _spellVFX.Initialize(_pooler, _spellSO.VFX, _spellSO.Duration, _spellSO.ImpactRadius);
            else _spellVFX.SetLifetime(_spellSO.Duration);
        }

        private void ApplyCooldown() => _currentCooldown = _spellSO.Cooldown;
    }
}