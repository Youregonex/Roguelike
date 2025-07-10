using UnityEngine;
using Yg.Battle;

namespace Yg.GameData.Perks
{
    [CreateAssetMenu(fileName = "PerkSO", menuName = "Perks/DamageTypeReductionPerkSO")]
    public class DamageTypeReductionPerkSO : PerkSO
    {
        [field: SerializeField, Range(0f, 1f)] public float DamageReductionPercent { get; private set; }
        [field: SerializeField] public EDamageType DamageType { get; private set; }

        public override Perk BuildPerk()
        {
            return new DamageTypeReductionPerk(this);
        }

        protected override void Validate()
        {
            Description = $"Reduce <b><color=#466C96>{DamageType}</color></b> damage taken by <b><color=#466C96>{DamageReductionPercent * 100}%</color></b>.";
        }
    }
}
