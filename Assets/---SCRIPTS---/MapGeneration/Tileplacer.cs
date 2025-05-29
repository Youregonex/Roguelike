using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using Y.Configs;

namespace Y.MapGeneration
{
    public class Tileplacer : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Tilemap _groundTilemap;
        [SerializeField] private Tile _defaultTile;

        private TileTypeToTileConfigSO _tileTypeToTileConfigSO;

        public void Initialize(TileTypeToTileConfigSO tileTypeToTileConfigSO)
        {
            _tileTypeToTileConfigSO = tileTypeToTileConfigSO;
        }

        public void PlaceGroundTiles(Dictionary<Vector2Int, ETileType> mapDictionary)
        {
            if(_tileTypeToTileConfigSO == null)
            {
                Debug.Log("TileTypeToTileConfig is null!");
                return;
            }
            
            foreach (var mapEntry in mapDictionary)
            {
                Tile tileToPlace = _tileTypeToTileConfigSO.GetTileFromType(mapEntry.Value);
                _groundTilemap.SetTile((Vector3Int)mapEntry.Key, tileToPlace);
            }
        }
    }
}
