using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    [CreateAssetMenu(fileName = "StatusEffectSO", menuName = "StatusEffects/DOT")]
    public class DOTStatusEffectSO : StatusEffectSO
    {
        [field: SerializeField] public float Interval { get; private set; }
        [field: SerializeField] public EDamageType DamageType { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }

        public override StatusEffect BuildStatusEffect(BattleUnitCore applier, BattleUnitCore holder, UltimatePooler pooler)
        {
            return new DOTStatusEffect(this, applier, holder, pooler);
        }

        protected override void Validate()
        {
            Description = $"Deals <b><color=#466C96>{Damage} {DamageType}</color></b> damage every <b><color=#466C96>{Interval}s</color></b>";
        }
    }
}
