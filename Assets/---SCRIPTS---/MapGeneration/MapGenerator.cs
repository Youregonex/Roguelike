using System.Collections.Generic;
using UnityEngine;
using Y.Configs;

namespace Y.MapGeneration
{
    public class MapGenerator
    {
        public Dictionary<Vector2Int, ETileType> GenerateMap(int width, int height, float scale, int seed, NoiseToTileTypeConfigSO noiseToTileTypeConfigSO)
        {
            Dictionary<Vector2Int, ETileType> mapDictionary = new();

            System.Random rng = new(seed);

            int offsetBoundries = 10000;

            float offsetX = rng.Next(-offsetBoundries, offsetBoundries);
            float offsetY = rng.Next(-offsetBoundries, offsetBoundries);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    float noiseValue = Mathf.PerlinNoise((x + offsetX) / scale, (y + offsetY) / scale);
                    noiseValue = Mathf.Clamp01(noiseValue);
                    ETileType tileType = noiseToTileTypeConfigSO.GetTiletypeWithNoise(noiseValue);
                    mapDictionary.Add(new Vector2Int(x, y), tileType);
                }

            return mapDictionary;
        }
    }
}