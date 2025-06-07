using UnityEngine;
using Yg.MapGeneration;
using Yg.PlayerCharacter;
using Zenject;
using System.Collections.Generic;

namespace Yg.EntryPoint
{
    public class GameplaySceneEntryPoint : MonoBehaviour
    {
        [CustomHeader("TEST")]
        [SerializeField] private Character _characterPrefab;

        private MapAssembler _mapAssembler;
        private Tileplacer _tileplacer;
        private TileGameObjectPlacer _tileGameObjectPlacer;
        private PointOfInterestPlacer _pointOfInterestPlacer;

        private DiContainer _diContainer;

        [Inject]
        private void Construct(
            DiContainer diContainer,
            MapAssembler mapAssembler,
            Tileplacer tileplacer,
            TileGameObjectPlacer tileGameObjectPlacer,
            PointOfInterestPlacer pointOfInterestPlacer)
        {
            _diContainer = diContainer;
            _mapAssembler = mapAssembler;
            _tileplacer = tileplacer;
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _pointOfInterestPlacer = pointOfInterestPlacer;
        }

        private void Awake()
        {
            InitializeScene();
        }

        private void InitializeScene()
        {
            Dictionary<Vector2Int, ETileType> mapDictionary = InitializeMapAssembler();
            InitializeTilePlacer(mapDictionary);
            InitializeTileGameObjectPlacer(mapDictionary);
            InitializePointOfInterestPlacer();

            SpawnPlayer();
        }

        private Dictionary<Vector2Int, ETileType> InitializeMapAssembler()
        {
            _mapAssembler.Initialize();
            return _mapAssembler.AssembleMap();
        }

        private void InitializeTilePlacer(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            _tileplacer.Initialize();
            _tileplacer.PlaceGroundTiles(mapDictionary);
        }

        private void InitializeTileGameObjectPlacer(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            _tileGameObjectPlacer.Initialize();
            _tileGameObjectPlacer.PlaceTilesGameObjects(mapDictionary);
        }

        private void InitializePointOfInterestPlacer()
        {
            _pointOfInterestPlacer.Initialize();
            _pointOfInterestPlacer.PlacePointsOfInterest();
        }

        private void SpawnPlayer()
        {
            Character character = _diContainer.InstantiatePrefab(_characterPrefab, new Vector2(0, 0), Quaternion.identity, null).GetComponent<Character>();
            character.Initialize(_tileGameObjectPlacer);
        }
    }
}