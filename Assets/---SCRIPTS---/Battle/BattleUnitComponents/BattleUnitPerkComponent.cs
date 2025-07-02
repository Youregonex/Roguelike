using System.Collections.Generic;
using System.Linq;
using Yg.GameData.Perks;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitPerkComponent : BattleUnitComponent
    {
        private BattleUnitStatsComponent _battleUnitStatsComponent;
        private List<Perk> _perkList = new();

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();

            if(!_battleUnitStatsComponent.IsInitialized)
                _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;

            for (int i = 0; i < _battleUnitStatsComponent.PerkList.Count; i++)
            {
                Perk perk = _battleUnitStatsComponent.PerkList[i].BuildPerk();
                _perkList.Add(perk);
            }
        }

        public void AddPerk(PerkSO perkSO)
        {
            if (_battleUnitStatsComponent.PerkList.Contains(perkSO)) return;

            _battleUnitStatsComponent.PerkList.Add(perkSO);
            Perk perk = perkSO.BuildPerk();
            _perkList.Add(perk);
        }

        public void RemovePerk(PerkSO perkSO)
        {
            if (!_battleUnitStatsComponent.PerkList.Contains(perkSO)) return;

            _battleUnitStatsComponent.PerkList.Remove(perkSO);
            _perkList.Remove(_perkList.Where(e => e.PerkSO == perkSO).FirstOrDefault());
        }

        public void ApplyPerks(EPerkApplicationEvent perkApplicationEvent, BattleUnitCore applier, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            List<Perk> perks = _perkList.Where(e => e.PerkSO.PerkApplicationEvent == perkApplicationEvent).ToList();

            foreach (var perk in perks)
                perk.ApplyPerk(applier, target, ref damageStruct);
        }
    }
}