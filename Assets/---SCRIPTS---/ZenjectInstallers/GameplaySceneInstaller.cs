using UnityEngine;
using Yg.MapGeneration;
using Yg.Player;
using Zenject;

namespace Yg.ZenjectInstallers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [CustomHeader("Systems")]
        [SerializeField] private Tileplacer _tilePlacer;
        [SerializeField] private TileGameObjectPlacer _tileGameObjectPlacer;
        [SerializeField] private MapAssembler _mapAssembler;
        [SerializeField] private PointOfInterestPlacer _pointOfInterestPlacer;
        [SerializeField] private PlayerSpawner _playerSpawner;


        public override void InstallBindings()
        {
            Container.Bind<Tileplacer>().FromInstance(_tilePlacer);
            Container.Bind<TileGameObjectPlacer>().FromInstance(_tileGameObjectPlacer);
            Container.Bind<MapAssembler>().FromInstance(_mapAssembler);
            Container.Bind<PointOfInterestPlacer>().FromInstance(_pointOfInterestPlacer);
            Container.Bind<PlayerSpawner>().FromInstance(_playerSpawner);
        }
    }
}
