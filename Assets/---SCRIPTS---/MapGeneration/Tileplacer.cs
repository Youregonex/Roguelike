using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class Tileplacer : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Tilemap _groundTilemap;
        [SerializeField] private Tilemap _pointOfInterestTilemap;
        [SerializeField] private Tile _defaultTile;

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
        }

        public void PlacePointOfInterestTile(Vector2Int position, Tile tile)
        {
            _groundTilemap.SetTile((Vector3Int)position, null);
            _pointOfInterestTilemap.SetTile((Vector3Int)position, tile);
        }
    }
}
