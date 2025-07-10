using Yg.Battle.BattleUnits;
using Yg.GameData.Units;
using Zenject;

namespace Yg.Factories
{
    public class BattleUnitFactory
    {
        private DiContainer _container;

        [Inject]
        public BattleUnitFactory(DiContainer container)
        {
            _container = container;
        }

        public BattleUnitCore CreateUnit(UnitDataSO unitDataSO)
        {
            return _container.InstantiatePrefab(unitDataSO.Prefab).GetComponent<BattleUnitCore>();
        }
    }
}
