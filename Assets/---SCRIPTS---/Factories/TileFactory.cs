using UnityEngine;
using Yg.MapGeneration;

namespace Yg.Factories
{
    public class TileFactory
    {
        private Transform _tileParentTransform;
        private BaseTile _tilePrefab;

        public TileFactory(Transform tileParentTransform, BaseTile tilePrefab)
        {
            _tileParentTransform = tileParentTransform;
            _tilePrefab = tilePrefab;
        }

        public BaseTile CreateTile(Vector2Int origin, ETileType tileType)
        {
            BaseTile baseTile = GameObject.Instantiate(
                _tilePrefab,
                new Vector2(origin.x, origin.y),
                Quaternion.identity,
                _tileParentTransform);

            bool walkableTile = tileType == ETileType.Mountain ? false : true;
            baseTile.Initialize(origin, tileType, walkableTile);
            return baseTile;
        }
    }
}
