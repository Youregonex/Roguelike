using UnityEngine;
using Y.MapGeneration;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
    [CustomHeader("Systems")]
    [SerializeField] private Tileplacer _tilePlacer;
    [SerializeField] private TileGameObjectPlacer _tileGameObjectPlacer;


    public override void InstallBindings()
    {
        Container.Bind<Tileplacer>().FromInstance(_tilePlacer);
        Container.Bind<TileGameObjectPlacer>().FromInstance(_tileGameObjectPlacer);
    }
}
