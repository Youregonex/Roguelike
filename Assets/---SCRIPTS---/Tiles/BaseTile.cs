using System.Collections.Generic;
using UnityEngine;
using System;

namespace Y.MapGeneration
{
    public class BaseTile : MonoBehaviour
    {
        public event Action<BaseTile> OnMouseHover;

        [CustomHeader("Settings")]
        [SerializeField] private GameObject _tileHoverHighlight;
        [field: SerializeField] public bool Walkable { get; private set; }

        private readonly List<Vector2Int> _neighbourDirections = new() {
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1)
        };

        public Vector2Int Origin { get; private set; }
        public ETileType TileType { get; private set; }

        public List<BaseTile> Neighbours { get; private set; } = new();
 
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        public BaseTile PreviousTile { get; private set; }

        public void Initialize(Vector2Int origin, ETileType tileType)
        {
            Origin = origin;
            TileType = tileType;

            _tileHoverHighlight.gameObject.SetActive(false);
        }

        public float GetDistanceToTile(BaseTile baseTile)
        {
            var dist = new Vector2Int(Mathf.Abs(Origin.x - baseTile.Origin.x), Mathf.Abs(Origin.y - baseTile.Origin.y));

            var lowest = Mathf.Min(dist.x, dist.y);
            var highest = Mathf.Max(dist.x, dist.y);

            var horizontalMovesRequired = highest - lowest;

            return lowest * 14 + horizontalMovesRequired * 10;
        }

        public void SetG(float g) => G = g;
        public void SetH(float h) => H = h;
        public void SetPreviousTile(BaseTile previousTile) => PreviousTile = previousTile;

        public void Highlight() => _tileHoverHighlight.SetActive(true);
        public void Unhighlight() => _tileHoverHighlight.SetActive(false);

        public void CacheNeighbours(MapAssembler mapAssembler)
        {
            Neighbours = new();
            foreach (var direction in _neighbourDirections)
            {
                BaseTile neighbour = mapAssembler.GetTileAtPosition(Origin + direction);

                if(neighbour != null)
                    Neighbours.Add(neighbour);
            }
        }

        private void OnMouseEnter()
        {
            _tileHoverHighlight.gameObject.SetActive(true);
            OnMouseHover?.Invoke(this);
        }

        private void OnMouseExit()
        {
            _tileHoverHighlight.gameObject.SetActive(false);
        }
    }
}
