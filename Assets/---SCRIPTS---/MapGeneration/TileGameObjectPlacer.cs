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

        private List<BaseTile> _tileList = new();
        private TileFactory _tileFactory;

        public void Initialize()
        {
            _tileFactory = new(_tileParentTransform, _baseTilePrefab);
        }

        public void PlaceTilesGameObjects(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            foreach (var mapEntry in mapDictionary)
            {
                BaseTile baseTile = _tileFactory.CreateTile(mapEntry.Key, mapEntry.Value);
                _tileList.Add(baseTile);
            }
        }
    }
}