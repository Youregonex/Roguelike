using UnityEngine;
using Yg.Battle.BattleUnits;
using Yg.Pooling;

namespace Yg.Battle
{
    public class DOTStatusEffect : StatusEffect
    {
        private DOTStatusEffectSO DOTStatusEffectSO => StatusEffectSO as DOTStatusEffectSO;
        private float _intervalTimer;

        public DOTStatusEffect(
            StatusEffectSO statusEffectSO,
            BattleUnitCore applier,
            BattleUnitCore holder,
            UltimatePooler pooler) : base(statusEffectSO, applier, holder, pooler)
        {
            if(statusEffectSO is not Battle.DOTStatusEffectSO)
            {
                Debug.LogError("Wrong StatusEffectSO");
                return;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            _intervalTimer = 0f;
        }

        public override void Tick()
        {
            _intervalTimer -= Time.deltaTime;

            if(_intervalTimer <= 0)
            {
                ApplyDamage();
                _intervalTimer = DOTStatusEffectSO.Interval;
            }
        }

        private void ApplyDamage()
        {
            DamageStruct damageStruct = new(
            _applierFaction,
            _statusEffectApplier,
            _statusEffectHolder.transform.position,
            EAttackType.StatusEffect,
            DOTStatusEffectSO.DamageType,
            DOTStatusEffectSO.Damage,
            0f);

            if (_statusEffectApplier is not null)
                _statusEffectApplier.DealDamage(damageStruct, _statusEffectHolder, true);
            else
                _statusEffectHolder.GetUnitComponent<BattleUnitHealthComponent>().TakeDamage(damageStruct);
        }
    }
}
