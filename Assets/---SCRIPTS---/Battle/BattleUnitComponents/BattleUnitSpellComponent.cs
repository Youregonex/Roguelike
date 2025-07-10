using System.Collections.Generic;
using UnityEngine;
using Yg.Pooling;
using Zenject;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitSpellComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _spellCheckInterval;

        private BattleUnitTargetComponent _battleUnitTargetComponent;
        private BattleUnitStatsComponent _battleUnitStatsComponent;
        private BattleUnitPerkComponent _battleUnitPerkComponent;

        private List<Spell> _spellList = new();
        private UltimatePooler _ultimatePooler;

        [Inject]
        private void Construct(UltimatePooler ultimatePooler)
        {
            _ultimatePooler = ultimatePooler;
        }

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitStatsComponent = _battleUnitCore.GetUnitComponent<BattleUnitStatsComponent>();
            _battleUnitTargetComponent = _battleUnitCore.GetUnitComponent<BattleUnitTargetComponent>();
            _battleUnitPerkComponent = _battleUnitCore.GetUnitComponent<BattleUnitPerkComponent>();

            if (_battleUnitStatsComponent.IsInitialized)
                BuildSpells();
            else
                _battleUnitStatsComponent.OnInitializationComplete += BattleUnitStatsComponent_OnInitializationComplete;

            float randomDelay = UnityEngine.Random.Range(0f, .5f);

            InvokeRepeating(nameof(TryCastSpell), randomDelay, _spellCheckInterval);
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
                if (spell.TryCast(_battleUnitTargetComponent.CurrentTarget))
                {
                    _battleUnitPerkComponent.ApplyPerks(EPerkApplicationEvent.OnSpellCast, _battleUnitTargetComponent.CurrentTarget);
                    return;
                }
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
                Spell spell = spellSO.BuildSpell(_battleUnitCore, _ultimatePooler);
                _spellList.Add(spell);
            }
        }
    }
}
