using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    public abstract class StatusEffect
    {
        public StatusEffectSO StatusEffectSO { get; private set; }

        protected StatusEffectVFXGameObject _vfxGameObject;
        protected BattleUnitCore _statusEffectApplier;
        protected BattleUnitCore _statusEffectHolder;
        protected UltimatePooler _pooler;
        protected float _currentDuration;
        protected EUnitFaction _applierFaction;

        public bool Expired => _currentDuration <= 0;

        public StatusEffect(StatusEffectSO statusEffectSO, BattleUnitCore applier, BattleUnitCore holder, UltimatePooler pooler)
        {
            StatusEffectSO = statusEffectSO;
            _currentDuration = statusEffectSO.Duration;
            _applierFaction = applier.UnitFaction;
            _statusEffectApplier = applier;
            _statusEffectHolder = holder;
            _pooler = pooler;
        }

        public virtual void Initialize()
        {
            InitializeStatusEffectVFX();
        }

        public virtual void Remove()
        {
            RemoveStatusEffectVFX();
        }

        public abstract void Tick();

        public virtual void DurationTick()
        {
            if (_currentDuration <= 0) return;

            _currentDuration -= Time.deltaTime;

            if (_currentDuration < 0)
                _currentDuration = 0f;
        }

        protected virtual void InitializeStatusEffectVFX()
        {
            _vfxGameObject = _pooler.Dequeue(StatusEffectSO.VFX);

            _vfxGameObject.transform.SetParent(_statusEffectHolder.transform);
            Vector2 slightYOffset = new(0f, -.1f);
            _vfxGameObject.transform.localPosition = Vector2.zero + slightYOffset;
        }

        protected virtual void RemoveStatusEffectVFX()
        {
            if (_vfxGameObject is not null)
                _pooler.Enqueue(StatusEffectSO.VFX, _vfxGameObject, true);
        }
    }
}