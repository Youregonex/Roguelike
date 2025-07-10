using Yg.Battle;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public abstract class Perk
    {
        public PerkSO PerkDataSO { get; private set; }

        public Perk(PerkSO perkSO)
        {
            PerkDataSO = perkSO;
        }

        public abstract void ApplyPerk(BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct);
    }
}
