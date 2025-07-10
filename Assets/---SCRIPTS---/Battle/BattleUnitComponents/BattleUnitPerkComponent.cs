using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.GameData.Perks;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitPerkComponent : BattleUnitComponent
    {
        private BattleUnitStatsComponent _battleUnitStatsComponent;
        private readonly HashSet<Perk> _perkSet = new();

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();

            if (!_battleUnitStatsComponent.IsInitialized)
                _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;
            else
                BuildPerksFromStats();
        }

        public void AddPerk(PerkSO perkSO)
        {
            if (_perkSet.Where(e => e.PerkDataSO.Name == perkSO.Name).Any()) return;

            Perk perk = perkSO.BuildPerk();
            _perkSet.Add(perk);
        }

        public void RemovePerk(PerkSO perkSO)
        {
            _perkSet.Remove(_perkSet.Where(e => e.PerkDataSO.Name == perkSO.Name).FirstOrDefault());
        }

        public void ApplyPerks(EPerkApplicationEvent perkApplicationEvent, BattleUnitCore target, ref DamageStruct damageStruct)
        {
            List<Perk> perks = _perkSet.Where(e => e.PerkDataSO.PerkApplicationEvent == perkApplicationEvent).ToList();

            foreach (var perk in perks)
                perk.ApplyPerk(_battleUnitCore, target, ref damageStruct);
        }

        public void ApplyPerks(EPerkApplicationEvent perkApplicationEvent, BattleUnitCore target)
        {
            List<Perk> perks = _perkSet.Where(e => e.PerkDataSO.PerkApplicationEvent == perkApplicationEvent).ToList();
            DamageStruct emptyStruct = new();

            foreach (var perk in perks)
                perk.ApplyPerk(_battleUnitCore, target, ref emptyStruct);
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
            BuildPerksFromStats();
        }

        private void BuildPerksFromStats()
        {
            for (int i = 0; i < _battleUnitStatsComponent.PerkSOList.Count; i++)
                AddPerk(_battleUnitStatsComponent.PerkSOList[i]);
        }
    }
}