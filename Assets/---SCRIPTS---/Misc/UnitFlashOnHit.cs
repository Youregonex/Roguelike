namespace Yg.Battle.BattleUnits
{
    public class UnitFlashOnHit : FlashOnHit
    {
        private BattleUnitHealthComponent _battleUnitHealthComponent;

        private void Start()
        {
            _battleUnitHealthComponent = Utilities.GetRootComponent<BattleUnitCore>(transform).GetUnitComponent<BattleUnitHealthComponent>();
            _battleUnitHealthComponent.OnDamageTaken += BattleUnitHealthComponent_OnDamageTaken;
        }

        private void OnDestroy()
        {
            _battleUnitHealthComponent.OnDamageTaken -= BattleUnitHealthComponent_OnDamageTaken;
            StopAllCoroutines();
        }

        private void BattleUnitHealthComponent_OnDamageTaken(DamageStruct damage)
        {
            if (_flashCoroutine is not null)
                StopAllCoroutines();

            StartCoroutine(FlashSprite());
        }
    }
}
