using UnityEngine;
using Yg.MapGeneration;
using Yg.Player;
using Zenject;
using System.Collections.Generic;
using Yg.GameData;

namespace Yg.EntryPoint
{
    public class GameplaySceneEntryPoint : MonoBehaviour
    {
        private PersistentData _persistentData;
        private MapAssembler _mapAssembler;
        private Tileplacer _tileplacer;
        private TileGameObjectPlacer _tileGameObjectPlacer;
        private PointOfInterestPlacer _pointOfInterestPlacer;
        private PlayerSpawner _playerSpawner;

        [Inject]
        private void Construct(
            PersistentData persistentData,
            MapAssembler mapAssembler,
            Tileplacer tileplacer,
            TileGameObjectPlacer tileGameObjectPlacer,
            PointOfInterestPlacer pointOfInterestPlacer,
            PlayerSpawner playerSpawner)
        {
            _mapAssembler = mapAssembler;
            _tileplacer = tileplacer;
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _pointOfInterestPlacer = pointOfInterestPlacer;
            _persistentData = persistentData;
            _playerSpawner = playerSpawner;
        }

        private void Awake()
        {
            InitializeScene();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                _persistentData.SaveData();

            if (Input.GetKeyDown(KeyCode.G))
                _persistentData.LoadData();
        }

        private void InitializeScene()
        {
            bool fromSaveData = false;

            if (!_persistentData.DataIsEmpty())
            {
                _persistentData.RestoreState();
                fromSaveData = true;
            }

            Dictionary<Vector2Int, ETileType> mapDictionary = InitializeMapAssembler(fromSaveData);
            InitializeTilePlacer(mapDictionary);
            InitializeTileGameObjectPlacer(mapDictionary);
            InitializePointOfInterestPlacer();
            InitializePlayerSpawner();
        }

        private Dictionary<Vector2Int, ETileType> InitializeMapAssembler(bool fromSaveData)
        {
            _mapAssembler.Initialize(fromSaveData);
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

        private void InitializePlayerSpawner()
        {
            _playerSpawner.Initialize();
            _playerSpawner.SpawnPlayer();
        }
    }
}