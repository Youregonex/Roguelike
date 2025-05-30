using UnityEngine;
using Y.MapGeneration;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
    [CustomHeader("Systems")]
    [SerializeField] private Tileplacer _tilePlacer;
    [SerializeField] private TileGameObjectPlacer _tileGameObjectPlacer;
    [SerializeField] private MapAssembler _mapAssembler;


    public override void InstallBindings()
    {
        Container.Bind<Tileplacer>().FromInstance(_tilePlacer);
        Container.Bind<TileGameObjectPlacer>().FromInstance(_tileGameObjectPlacer);
        Container.Bind<MapAssembler>().FromInstance(_mapAssembler);
    }
}
