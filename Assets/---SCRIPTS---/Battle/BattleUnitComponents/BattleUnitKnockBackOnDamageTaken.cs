using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitKnockBackOnDamageTaken : BattleUnitComponent
    {
        private BattleUnitHealthComponent _battleUnitHealthComponent;
        private Rigidbody2D _rigidbody;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitHealthComponent = _battleUnitCore.GetUnitComponent<BattleUnitHealthComponent>();
            _battleUnitHealthComponent.OnDamageTaken += BattleUnitHealthComponent_OnDamageTaken;

            _rigidbody = transform.root.GetComponent<Rigidbody2D>();
        }

        private void OnDestroy()
        {
            _battleUnitHealthComponent.OnDamageTaken -= BattleUnitHealthComponent_OnDamageTaken;
        }

        private void BattleUnitHealthComponent_OnDamageTaken(DamageStruct damage)
        {
            if (damage.Origin is null) return;
            var knockBackDirection = Utilities.GetDirectionVectorNormalized(transform.position, damage.Origin.transform.position, true);
            var knockBackForce = knockBackDirection * damage.KnockBackForce;
            _rigidbody.AddForce(knockBackForce, ForceMode2D.Impulse);
        }
    }
}
