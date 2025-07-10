using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public class OnAttackApplyStatusEffectPerk : Perk
    {
        protected OnAttackApplyStatusEffectPerkSO OnAttackApplyStatusEffectSO => PerkDataSO as OnAttackApplyStatusEffectPerkSO;

        public OnAttackApplyStatusEffectPerk(PerkSO perkSO) : base(perkSO) {}

        public override void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            if (target is null) return;
            OnAttackApplyStatusEffectSO.StatusEffectSO.ApplyStatusEffect(applier, target);
        }
    }
}
