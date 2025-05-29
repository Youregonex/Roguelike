using System.Collections.Generic;
using UnityEngine;
using Y.Configs;
using Zenject;

namespace Y.MapGeneration
{
    public class MapAssembler : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seedString;
        [SerializeField] private float _noiseScale;
        
        private const string MAP_GENERATION_CONFIG_PATH = "Configs/MapGeneration/MainMapGenerationConfig";
        private const string NOISE_TO_TILE_TYPE_CONFIG_PATH = "Configs/Tiles/MainNoiseToTileTypeConfig";
        private const string TILE_TYPE_TO_TILE_CONFIG_PATH = "Configs/Tiles/MainTileTypeToTileConfig";

        private DefaultMapGenerationConfigSO _defaultMapGenerationConfigSO;
        private NoiseToTileTypeConfigSO _noiseToTileTypeConfigSO;
        private TileTypeToTileConfigSO _tileTypeToTileConfigSO;

        private Tileplacer _tilePlacer;
        private MapGenerator _mapGenerator;
        private TileGameObjectPlacer _tileGameObjectPlacer;

        private int _seed;

        public Dictionary<Vector2Int, ETileType> MapDictionary { get; private set; }

        [Inject]
        private void Construct(Tileplacer tilePlacer, TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tilePlacer = tilePlacer;
            _tileGameObjectPlacer = tileGameObjectPlacer;
        }

        public void Initialize()
        {
            _defaultMapGenerationConfigSO = Resources.Load<DefaultMapGenerationConfigSO>(MAP_GENERATION_CONFIG_PATH);
            _noiseToTileTypeConfigSO = Resources.Load<NoiseToTileTypeConfigSO>(NOISE_TO_TILE_TYPE_CONFIG_PATH);
            _tileTypeToTileConfigSO = Resources.Load<TileTypeToTileConfigSO>(TILE_TYPE_TO_TILE_CONFIG_PATH);

            _seed = _useRandomSeed ? UnityEngine.Random.Range(int.MinValue, int.MaxValue) : _seedString.GetHashCode();

            _mapGenerator = new();
        }

        private void Awake()
        {
            Initialize();
            AssembleMap();

            _tilePlacer.Initialize(_tileTypeToTileConfigSO);
            _tilePlacer.PlaceGroundTiles(MapDictionary);

            _tileGameObjectPlacer.Initialize();
            _tileGameObjectPlacer.PlaceTilesGameObjects(MapDictionary);
        }

        public void AssembleMap()
        {
            if (_mapGenerator != null)
                MapDictionary = _mapGenerator.GenerateMap(
                    _defaultMapGenerationConfigSO.MapWidth,
                    _defaultMapGenerationConfigSO.MapHeight,
                    _noiseScale,
                    _seed,
                    _noiseToTileTypeConfigSO);
            else
                Debug.Log("Map Generator is null");
        }
    }
}

