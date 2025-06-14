using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Yg.GameConfigs;
using Yg.SaveLoad;

namespace Yg.MapGeneration
{
    public class MapAssembler : MonoBehaviour, ISaveable
    {
        [CustomHeader("Settings")]
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seedString;
        [SerializeField] private float _noiseScale;

        private DefaultMapGenerationConfigSO _mapGenerationConfig;
        private NoiseToTileTypeConfigSO _noiseToTileTypeConfig;
        private MapGenerator _mapGenerator;

        private int _currentLevel;
        private int _seed;

        public Dictionary<Vector2Int, ETileType> MapDictionary { get; private set; } = new();

        public void Initialize(bool fromSaveData)
        {
            if(!fromSaveData)
            {
                _seed = _useRandomSeed ? UnityEngine.Random.Range(int.MinValue, int.MaxValue) : _seedString.GetHashCode();
                _currentLevel = 1;
            }

            _mapGenerator = new();

            _mapGenerationConfig = ResourceLoader.CONFIG_MapGeneration;
            _noiseToTileTypeConfig = ResourceLoader.CONFIG_NoiseToTileType;
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

        public object CaptureState()
        {
            MapAssemblerSaveData mapAssemblerSaveData = new()
            {
                CurrentLevel = _currentLevel,
                CurrentSeed = _seed
            };

            return mapAssemblerSaveData;
        }

        public void RestoreState(object data)
        {
            var mapAssemblerSaveData = data as MapAssemblerSaveData
                ?? JsonConvert.DeserializeObject<MapAssemblerSaveData>(JsonConvert.SerializeObject(data));

            _currentLevel = mapAssemblerSaveData.CurrentLevel;
            _seed = mapAssemblerSaveData.CurrentSeed;
        }
    }

    [System.Serializable]
    public class MapAssemblerSaveData
    {
        public int CurrentLevel;
        public int CurrentSeed;
    }
}