using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitSpellComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _spellCheckInterval;

        private BattleUnitStatsComponent _battleUnitStatsComponent;
        private List<Spell> _spellList = new();

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();

            if (_battleUnitStatsComponent.SpellSOList is null)
                _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;
            else
                BuildSpells();

            float randomDelay = UnityEngine.Random.Range(0f, .5f);

            InvokeRepeating("TryCastSpell", randomDelay, _spellCheckInterval);
        }

        public void Tick()
        {
            foreach (var spell in _spellList)
            {
                if (spell.OnCooldown)
                    spell.CooldownTick();
            }
        }

        private void TryCastSpell()
        {
            foreach (var spell in _spellList)
            {
                if (spell.OnCooldown) continue;
                if (spell.TryCast(_battleUnitCore))
                    return;
            }
        }

        private void BattleUnitStatsComponent_OnInitializationComplete()
        {
            _battleUnitStatsComponent.OnInitializationComplete -= BattleUnitStatsComponent_OnInitializationComplete;
            BuildSpells();
        }

        private void BuildSpells()
        {
            foreach (var spellSO in _battleUnitStatsComponent.SpellSOList)
            {
                Spell spell = spellSO.BuildSpell();
                _spellList.Add(spell);
            }
        }
    }
}
