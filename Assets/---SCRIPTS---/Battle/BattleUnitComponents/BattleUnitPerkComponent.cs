using System.Collections.Generic;
using System.Linq;
using Yg.GameData.Perks;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitPerkComponent : BattleUnitComponent
    {
        private BattleUnitStatsComponent _battleUnitStatsComponent;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();
        }

        public void AddPerk(Perk perk)
        {
            if (!_battleUnitStatsComponent.PerkList.Contains(perk))
                _battleUnitStatsComponent.PerkList.Add(perk);
        }

        public void RemovePerk(Perk perk)
        {
            if (_battleUnitStatsComponent.PerkList.Contains(perk))
                _battleUnitStatsComponent.PerkList.Remove(perk);
        }

        public void ApplyPerks(EPerkApplicationEvent perkApplicationEvent, ref DamageStruct damageStruct)
        {
            List<Perk> perks = _battleUnitStatsComponent.PerkList.Where(e => e.PerkApplicationEvent == perkApplicationEvent).ToList();

            foreach (var perk in perks)
                perk.ApplyPerk(ref damageStruct);
        }
    }
}