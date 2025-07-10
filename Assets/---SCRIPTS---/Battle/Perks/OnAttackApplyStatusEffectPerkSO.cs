using UnityEngine;
using Yg.Battle;

namespace Yg.GameData.Perks
{
    [CreateAssetMenu(fileName = "PerkSO", menuName = "Perks/OnAttackApplyStatusPerk")]
    public class OnAttackApplyStatusEffectPerkSO : PerkSO
    {
        [field: SerializeField] public StatusEffectSO StatusEffectSO { get; private set; }

        public override Perk BuildPerk()
        {
            return new OnAttackApplyStatusEffectPerk(this);
        }

        protected override void Validate()
        {
            Description = $"On attack apply <b><color=#466C96>{StatusEffectSO.Name}</color></b>";
        }
    }
}
