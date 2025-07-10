using UnityEngine;
using Yg.Battle;

namespace Yg.GameData.Perks
{
    [CreateAssetMenu(fileName = "PerkSO", menuName = "Perks/DamageOnDamageDealtPerkSO")]
    public class DamageOnDamageDealtPerkSO : PerkSO
    {
        [field: SerializeField] public EDamageType DamageType { get; protected set; }
        [field: SerializeField] public float Damage { get; protected set; }

        public override Perk BuildPerk()
        {
            return new DamageOnDamageDealtPerk(this);
        }

        protected override void Validate()
        {
            Description = $"On attack deals additional <b><color=#466C96>{Damage}</color></b> <b><color=#466C96>{DamageType}</color></b> damage.";
        }
    }
}