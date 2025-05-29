using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

namespace Y.Configs
{
    [CreateAssetMenu(fileName = "TileTypeToTileConfigSO", menuName = "Configs/Tiles/TileTypeToTileConfigSO")]
    public class TileTypeToTileConfigSO : ScriptableObject
    {
        [field: SerializeField] public List<TypeToTile> TypeToTileList { get; private set; }

        public Tile GetTileFromType(ETileType tileType)
        {
            Tile tile = TypeToTileList.Where(entry => entry.TileType == tileType).First().Tile;

            if (tile == null)
            {
                Debug.Log($"Couldn't find proper tile for {tileType} TileType!");
                return null;
            }

            return tile;
        }
    }

    [System.Serializable]
    public class TypeToTile
    {
        [field: SerializeField] public ETileType TileType { get; private set; }
        [field: SerializeField] public Tile Tile { get; private set; }
    }
}
