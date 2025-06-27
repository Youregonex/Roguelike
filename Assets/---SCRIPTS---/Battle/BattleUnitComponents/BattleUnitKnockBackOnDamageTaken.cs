using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitKnockBackOnDamageTaken : BattleUnitComponent
    {
        private BattleUnitHealthComponent _battleUnitHealthComponent;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitHealthComponent = _battleUnitCore.GetUnitComponent<BattleUnitHealthComponent>();
            _battleUnitHealthComponent.OnDamageTaken += BattleUnitHealthComponent_OnDamageTaken;
        }

        private void OnDestroy()
        {
            _battleUnitHealthComponent.OnDamageTaken -= BattleUnitHealthComponent_OnDamageTaken;
        }

        private void BattleUnitHealthComponent_OnDamageTaken(DamageStruct damage)
        {
            Rigidbody2D rigidbody = transform.root.GetComponent<Rigidbody2D>();
            var knockBackDirection = Utilities.GetDirectionVectorNormalized(transform.position, damage.Origin.position, true);
            var knockBackForce = knockBackDirection * damage.KnockBackForce;
            rigidbody.AddForce(knockBackForce, ForceMode2D.Impulse);
        }
    }
}
