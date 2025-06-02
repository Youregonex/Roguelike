using System.Collections.Generic;
using UnityEngine;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class MapAssembler : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seedString;
        [SerializeField] private float _noiseScale;

        private DefaultMapGenerationConfigSO _mapGenerationConfig;
        private NoiseToTileTypeConfigSO _noiseToTileTypeConfig;
        private MapGenerator _mapGenerator;

        private int _seed;

        public Dictionary<Vector2Int, ETileType> MapDictionary { get; private set; } = new();

        public void Initialize()
        {
            _seed = _useRandomSeed ? UnityEngine.Random.Range(int.MinValue, int.MaxValue) : _seedString.GetHashCode();
            _mapGenerator = new();

            _mapGenerationConfig = ConfigLoader.MapGenerationConfig;
            _noiseToTileTypeConfig = ConfigLoader.NoiseToTileTypeConfig;
        }

        public Dictionary<Vector2Int, ETileType> AssembleMap()
        {
            if (_mapGenerator != null)
                MapDictionary = _mapGenerator.GenerateMap(
                    _mapGenerationConfig.MapWidth,
                    _mapGenerationConfig.MapHeight,
                    _noiseScale,
                    _seed,
                    _noiseToTileTypeConfig);
            else
                Debug.Log("Map Generator is null");

            return MapDictionary;
        }

        public bool WithinBounds(Vector2Int position) => MapDictionary.ContainsKey(position);
    }
}