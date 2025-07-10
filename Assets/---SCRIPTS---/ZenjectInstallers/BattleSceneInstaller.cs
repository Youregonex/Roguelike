using UnityEngine;
using Yg.Battle;
using Yg.Battle.GameSystems;
using Yg.Factories;
using Yg.Pooling;
using Yg.UI;
using Zenject;

namespace Yg.ZenjectInstallers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [CustomHeader("Settings")]
        [SerializeField] private BattleUnitSpawner _unitSpawner;
        [SerializeField] private SquadPlacementUI _squadPlacementUI;
        [SerializeField] private BattleSquadPlacer _battleSquadPlacer;

        [CustomHeader("PoolParent")]
        [SerializeField] private Transform _poolParent;


        public override void InstallBindings()
        {
            Container.Bind<BattleUnitSpawner>().FromInstance(_unitSpawner);
            Container.Bind<SquadPlacementUI>().FromInstance(_squadPlacementUI);
            Container.Bind<BattleSquadPlacer>().FromInstance(_battleSquadPlacer);

            Container.Bind<UltimatePooler>().AsSingle().WithArguments(_poolParent);
            Container.Bind<UnitRegistry>().AsSingle().NonLazy();

            BindFactories();
        }

        private void BindFactories()
        {
            Container.Bind<BattleUnitFactory>().AsTransient();
        }
    }
}
