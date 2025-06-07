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
        [SerializeField] private Tilemap _pointOfInterestTilemap;
        [SerializeField] private Tilemap _fowTilemap;

        [CustomHeader("Tiles")]
        [SerializeField] private Tile _defaultTile;
        [SerializeField] private Tile _fowUnvisitedTile;
        [SerializeField] private Tile _fowVisitedTile;

        private TileTypeToTileConfigSO _tileTypeToTileConfig;

        public void Initialize()
        {
            _tileTypeToTileConfig = ConfigLoader.TileTypeToTileConfig;
        }

        public void PlaceGroundTiles(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            if(_tileTypeToTileConfig == null)
            {
                Debug.Log("TileTypeToTileConfig is null!");
                return;
            }
            
            foreach (var mapEntry in mapDictionary)
            {
                Tile tileToPlace = _tileTypeToTileConfig.GetTileFromType(mapEntry.Value);
                _groundTilemap.SetTile((Vector3Int)mapEntry.Key, tileToPlace);
            }

            PlaceInitialFOW(mapDictionary);
        }

        private void PlaceInitialFOW(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            foreach (var mapEntry in mapDictionary)
                PlaceUnvisiterFOW(mapEntry.Key);
        }

        public void PlacePointOfInterestTile(Vector2Int position, Tile tile)
        {
            _groundTilemap.SetTile((Vector3Int)position, null);
            _pointOfInterestTilemap.SetTile((Vector3Int)position, tile);
        }

        public void PlaceUnvisiterFOW(Vector2Int position)
        {
            _fowTilemap.SetTile((Vector3Int)position, _fowUnvisitedTile);
        }

        public void PlaceVisitedFOW(Vector2Int position)
        {
            _fowTilemap.SetTile((Vector3Int)position, _fowVisitedTile);
        }

        public void RemoveFOW(Vector2Int position)
        {
            _fowTilemap.SetTile((Vector3Int)position, null);
        }
    }
}
