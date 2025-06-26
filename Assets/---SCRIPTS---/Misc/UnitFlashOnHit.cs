namespace Yg.Battle.BattleUnits
{
    public class UnitFlashOnHit : FlashOnHit
    {
        private BattleUnitHealthComponent _battleUnitHealthComponent;

        private void Start()
        {
            _battleUnitHealthComponent = transform.root.GetComponent<BattleUnitCore>().GetUnitComponent<BattleUnitHealthComponent>();
            _battleUnitHealthComponent.OnDamageTaken += BattleUnitHealthComponent_OnDamageTaken;
        }

        private void OnDestroy()
        {
            _battleUnitHealthComponent.OnDamageTaken -= BattleUnitHealthComponent_OnDamageTaken;
            StopAllCoroutines();
        }

        private void BattleUnitHealthComponent_OnDamageTaken()
        {
            if (_flashCoroutine is not null)
                StopAllCoroutines();

            StartCoroutine(FlashSprite());
        }
    }
}
