using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public abstract class Perk
    {
        public PerkSO PerkSO { get; private set; }


        public Perk(PerkSO perkSO)
        {
            PerkSO = perkSO;
        }

        public abstract void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct);
    }
}
