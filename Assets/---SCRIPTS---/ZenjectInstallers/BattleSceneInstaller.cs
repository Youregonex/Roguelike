using UnityEngine;
using Yg.Battle;
using Yg.Battle.GameSystems;
using Yg.UI;
using Zenject;

namespace Yg.ZenjectInstallers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [CustomHeader("Settings")]
        [SerializeField] private BattleUnitSpawner _unitSpawner;
        [SerializeField] private SquadPlacementUI _squadPlacementUI;
        [SerializeField] private EnemySquadPlacer _enemySquadPlacer;

        public override void InstallBindings()
        {
            Container.Bind<BattleUnitSpawner>().FromInstance(_unitSpawner);
            Container.Bind<SquadPlacementUI>().FromInstance(_squadPlacementUI);
            Container.Bind<EnemySquadPlacer>().FromInstance(_enemySquadPlacer);
        }
    }
}
