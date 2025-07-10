using UnityEngine;
using Yg.Battle;

namespace Yg.GameData.Perks
{
    [CreateAssetMenu(fileName = "PerkSO", menuName = "Perks/AttackTypeReductionPerkSO")]
    public class AttackTypeReductionPerkSO : PerkSO
    {
        [field: SerializeField, Range(0f, 1f)] public float DamageReductionPercent { get; private set; }
        [field: SerializeField] public EAttackType DefenceFromAttackType { get; private set; }

        public override Perk BuildPerk()
        {
            return new AttackTypeReductionPerk(this);
        }

        protected override void Validate()
        {
            Description = $"Reduce <b><color=#466C96>{DefenceFromAttackType}</color></b> damage taken by <b><color=#466C96>{DamageReductionPercent * 100}%</color></b>.";
        }
    }
}
