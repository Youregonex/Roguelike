using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    public abstract class StatusEffectSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, TextArea(3, 10)] public string Description { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public bool Tickable { get; private set; }
        [field: SerializeField] public StatusEffectVFXGameObject VFX { get; private set; }

        public abstract StatusEffect BuildStatusEffect(BattleUnitCore applier, BattleUnitCore target, UltimatePooler pooler);

        public virtual void ApplyStatusEffect(BattleUnitCore applier, BattleUnitCore target)
        {
            if (target.TryGetUnitComponent(out BattleUnitStatusEffectComponent battleUnitStatusEffectComponent))
                battleUnitStatusEffectComponent.ApplyStatusEffect(applier, this);
            else
                Debug.LogError("Couldn't find StatusEffectComponent");
        }

        protected abstract void Validate();

        protected void OnValidate()
        {
            Validate();
        }
    }
}
