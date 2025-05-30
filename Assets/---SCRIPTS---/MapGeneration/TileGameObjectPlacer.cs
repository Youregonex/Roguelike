using System.Collections.Generic;
using UnityEngine;
using Y.Factories;

namespace Y.MapGeneration
{
    public class TileGameObjectPlacer : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Transform _tileParentTransform;
        [SerializeField] private BaseTile _baseTilePrefab;
        [SerializeField] private BaseTile _baseTileUnwalkablePrefab;

        private TileFactory _tileFactory;

        public void Initialize()
        {
            _tileFactory = new(_tileParentTransform, _baseTilePrefab, _baseTileUnwalkablePrefab);
        }

        public List<BaseTile> PlaceTilesGameObjects(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            List<BaseTile> tileList = new();

            foreach (var mapEntry in mapDictionary)
            {
                BaseTile baseTile = _tileFactory.CreateTile(mapEntry.Key, mapEntry.Value);
                tileList.Add(baseTile);
            }

            return tileList;
        }
    }
}