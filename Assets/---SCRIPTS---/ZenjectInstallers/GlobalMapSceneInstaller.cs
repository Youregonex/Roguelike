using UnityEngine;
using Yg.MapGeneration;
using Yg.Character;
using Yg.UI;
using Zenject;
using Yg.Systems;
using Yg.GameData.Equipment;

namespace Yg.ZenjectInstallers
{
    public class GlobalMapSceneInstaller : MonoInstaller
    {
        [CustomHeader("Systems")]
        [SerializeField] private Tileplacer _tilePlacer;
        [SerializeField] private TileGameObjectPlacer _tileGameObjectPlacer;
        [SerializeField] private MapAssembler _mapAssembler;
        [SerializeField] private PointOfInterestPlacer _pointOfInterestPlacer;
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private BattleInitiator _battleInitiator;

        [CustomHeader("UI Components")]
        [SerializeField] private MovementUI _movementUI;
        [SerializeField] private WarbandUI _warbandUI;
        [SerializeField] private RecruitmentUI _recruitmentUI;
        [SerializeField] private TooltipDrawer _tooltipDrawer;


        public override void InstallBindings()
        {
            Container.Bind<Tileplacer>().FromInstance(_tilePlacer);
            Container.Bind<TileGameObjectPlacer>().FromInstance(_tileGameObjectPlacer);
            Container.Bind<MapAssembler>().FromInstance(_mapAssembler);
            Container.Bind<PointOfInterestPlacer>().FromInstance(_pointOfInterestPlacer);
            Container.Bind<PlayerSpawner>().FromInstance(_playerSpawner);
            Container.Bind<BattleInitiator>().FromInstance(_battleInitiator);

            Container.Bind<EquipmentBuilder>().AsSingle();

            Container.Bind<MovementUI>().FromInstance(_movementUI);
            Container.Bind<WarbandUI>().FromInstance(_warbandUI);
            Container.Bind<RecruitmentUI>().FromInstance(_recruitmentUI);
            Container.Bind<TooltipDrawer>().FromInstance(_tooltipDrawer);
        }
    }
}
