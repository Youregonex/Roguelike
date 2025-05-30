using UnityEngine;
using Y.MapGeneration;

namespace Y.Factories
{
    public class TileFactory
    {
        private Transform _tileParentTransform;
        private BaseTile _tilePrefab;
        private BaseTile _tileUnwalkablePrefab;

        public TileFactory(Transform tileParentTransform, BaseTile tilePrefab, BaseTile tileUnwalkablePrefab)
        {
            _tileParentTransform = tileParentTransform;
            _tilePrefab = tilePrefab;
            _tileUnwalkablePrefab = tileUnwalkablePrefab;
        }

        public BaseTile CreateTile(Vector2Int origin, ETileType tileType)
        {
            BaseTile prefab = tileType == ETileType.Mountain ? _tileUnwalkablePrefab : _tilePrefab;
            BaseTile baseTile = GameObject.Instantiate(
                prefab,
                new Vector2(origin.x, origin.y),
                Quaternion.identity,
                _tileParentTransform);

            baseTile.Initialize(origin, tileType);
            return baseTile;
        }
    }
}
