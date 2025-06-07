using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.Factories;
using System;

namespace Yg.MapGeneration
{
    public class TileGameObjectPlacer : MonoBehaviour
    {
        public event Action<BaseTile> OnTileHighlight;

        [CustomHeader("Settings")]
        [SerializeField] private Transform _tileParentTransform;
        [SerializeField] private BaseTile _baseTilePrefab;

        private TileFactory _tileFactory;

        public List<BaseTile> MapBaseTileList { get; private set; } = new();

        public void Initialize()
        {
            _tileFactory = new(_tileParentTransform, _baseTilePrefab);
        }

        private void OnDestroy()
        {
            foreach (var tile in MapBaseTileList)
                tile.OnMouseHover -= BaseTile_OnMouseHover;   
        }

        public void PlaceTilesGameObjects(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            foreach (var mapEntry in mapDictionary)
            {
                BaseTile baseTile = _tileFactory.CreateTile(mapEntry.Key, mapEntry.Value);
                baseTile.OnMouseHover += BaseTile_OnMouseHover;
                MapBaseTileList.Add(baseTile);
            }

            foreach (var baseTile in MapBaseTileList)
                baseTile.CacheNeighbours(this);
        }

        public void HighlightTiles(List<BaseTile> tiles)
        {
            foreach (var tile in tiles)
                tile.Highlight();
        }

        public void UnhighlightTiles(List<BaseTile> tiles)
        {
            foreach (var tile in tiles)
                tile.Unhighlight();
        }

        private void BaseTile_OnMouseHover(BaseTile hoveredTile)
        {
            OnTileHighlight?.Invoke(hoveredTile);
        }

        public void AssignPointOfInterestToTileAtPosition(Vector2Int position, IPointOfInterest pointOfInterest)
        {
            GetTileAtPosition(position).AssignPointOfInterest(pointOfInterest);
        }

        public BaseTile GetTileAtPosition(Vector2Int position)
        {
            return MapBaseTileList.Where(entry => entry.Origin == position).FirstOrDefault();
        }
    }
}