
namespace Yg.Battle.BattleUnits
{
    public class BattleUnitKnockBackOnDamageTaken : BattleUnitComponent
    {
        private BattleUnitHealthComponent _battleUnitHealthComponent;
        private BattleUnitMovementComponent _battleUnitMovementComponent;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _battleUnitHealthComponent = _battleUnitCore.GetUnitComponent<BattleUnitHealthComponent>();
            _battleUnitHealthComponent.OnDamageTaken += BattleUnitHealthComponent_OnDamageTaken;

            _battleUnitMovementComponent = _battleUnitCore.GetUnitComponent<BattleUnitMovementComponent>();
        }

        private void OnDestroy()
        {
            _battleUnitHealthComponent.OnDamageTaken -= BattleUnitHealthComponent_OnDamageTaken;
        }

        private void BattleUnitHealthComponent_OnDamageTaken(DamageStruct damage)
        {
            var direction = Utilities.GetDirectionVectorNormalized(
                transform.position,
                damage.OriginPosition,
                true);

            var force = direction * damage.KnockBackForce;
            _battleUnitMovementComponent.AddKnockback(force);
        }
    }
}
