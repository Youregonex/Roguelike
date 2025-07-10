using System;
using System.Collections.Generic;
using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    public abstract class SpellSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string SpellId { get; private set; }
        [field: SerializeField, TextArea(3, 10)] public string Description { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public List<ETag> Tags { get; private set; }

        [field: Space(10f)]

        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public float ActivationRange { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public float ImpactRadius { get; private set; }
        [field: SerializeField] public EDamageType AffectionType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public float KnockbackValue { get; private set; }
        [field: SerializeField] public List<StatusEffectSO> StatusEffectList { get; private set; }
        [field: SerializeField] public SpellVFXGameObject VFX { get; private set; }

        public abstract Spell BuildSpell(BattleUnitCore caster, UltimatePooler pooler);

        public void GenerateId() => SpellId = Guid.NewGuid().ToString();

        protected virtual void Validate()
        {
            if (string.IsNullOrEmpty(SpellId)) GenerateId();
        }

        protected void OnValidate()
        {
            Validate();
        }
    }
}
