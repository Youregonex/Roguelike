using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class Tileplacer : MonoBehaviour
    {
        [CustomHeader("Tilemaps")]
        [SerializeField] private Tilemap _groundTilemap;
        [SerializeField] private Tilemap _environmentTilemap;
        [SerializeField] private Tilemap _pointOfInterestTilemap;
        [SerializeField] private Tilemap _fowTilemap;

        [CustomHeader("Tiles")]
        [SerializeField] private Tile _defaultTile;
        [SerializeField] private Tile _fowUnvisitedTile;
        [SerializeField] private Tile _fowVisitedTile;

        private TileTypeToTileConfigSO _tileTypeToTileConfig;

        public void Initialize()
        {
            _tileTypeToTileConfig = ResourceLoader.CONFIG_TileTypeToTile;
        }

        public void PlaceGroundTiles(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            if(_tileTypeToTileConfig == null)
            {
                Debug.Log("TileTypeToTileConfig is null!");
                return;
            }

            PlaceInitialGroundTiles(mapDictionary);

            foreach (var mapEntry in mapDictionary)
            {
                if (mapEntry.Value == ETileType.Meadow) continue;

                Tile tileToPlace = _tileTypeToTileConfig.GetTileFromType(mapEntry.Value);
                _environmentTilemap.SetTile((Vector3Int)mapEntry.Key, tileToPlace);
            }

            PlaceInitialFOW(mapDictionary);
        }

        public void PlacePointOfInterestTile(Vector2Int position, Tile tile)
        {
            _environmentTilemap.SetTile((Vector3Int)position, null);
            _pointOfInterestTilemap.SetTile((Vector3Int)position, tile);
        }

        public void PlaceVisitedFOW(Vector2Int position)
        {
            _fowTilemap.SetTile((Vector3Int)position, _fowVisitedTile);
        }

        public void RemoveFOW(Vector2Int position)
        {
            _fowTilemap.SetTile((Vector3Int)position, null);
        }

        private void PlaceInitialFOW(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            foreach (var mapEntry in mapDictionary)
                PlaceUnvisiterFOW(mapEntry.Key);
        }

        private void PlaceUnvisiterFOW(Vector2Int position)
        {
            _fowTilemap.SetTile((Vector3Int)position, _fowUnvisitedTile);
        }

        private void PlaceInitialGroundTiles(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            foreach (var mapEntry in mapDictionary)
            {
                Tile tileToPlace = _tileTypeToTileConfig.GetTileFromType(ETileType.Meadow);
                _groundTilemap.SetTile((Vector3Int)mapEntry.Key, tileToPlace);
            }
        }
    }
}
