using UnityEngine;

namespace Yg.Battle
{
    [CreateAssetMenu(fileName = "AOEDamageSpell", menuName = "Spells/AOE/Damage")]
    public class AOEDamageSpellSO : AOESpellSO
    {
        [field: SerializeField] public EDamageType DamageType { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public GameObject SpellVFX { get; private set; }

        private void OnValidate()
        {
            Description = $"Deals <b><color=#466B95>{Damage}</color></b> <b><color=#466B95>{DamageType}</color></b> damage in <b><color=#466B95>{ImpactRadius}m</color></b> range.";
        }

        public override Spell BuildSpell()
        {
            return new AOEDamageSpell(this);
        }
    }
}
