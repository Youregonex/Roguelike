using System.Collections.Generic;
using UnityEngine;
using System;

namespace Yg.MapGeneration
{
    public class BaseTile : MonoBehaviour
    {
        public event Action<BaseTile> OnMouseHover;

        [CustomHeader("Settings")]
        [SerializeField] private GameObject _mouseHighlight;
        [SerializeField] private GameObject _pathHighlight;

        private const int  DEFAULT_MOVE_COST = 10;
        private const int  DIAGONAL_MOVE_COST = 14;

        public bool Walkable { get; private set; } = true;
        public bool PlayerWalkable => Walkable && IsRevealed;

        private IPointOfInterest _pointOfInterest;

        public Vector2Int Origin { get; private set; }
        public ETileType TileType { get; private set; }

        // A*
        public List<BaseTile> Neighbours { get; private set; } = new();
 
        public BaseTile PreviousTile { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        // FOW
        public bool IsRevealed { get; private set; } = false;

        public void Initialize(Vector2Int origin, ETileType tileType, bool isWalkable = true)
        {
            Origin = origin;
            TileType = tileType;
            Walkable = isWalkable;

            _pathHighlight.SetActive(false);
            _mouseHighlight.SetActive(false);
        }

        public float GetDistanceToTile(BaseTile baseTile)
        {
            var dist = new Vector2Int(Mathf.Abs(Origin.x - baseTile.Origin.x), Mathf.Abs(Origin.y - baseTile.Origin.y));

            var lowest = Mathf.Min(dist.x, dist.y);
            var highest = Mathf.Max(dist.x, dist.y);

            var horizontalMovesRequired = highest - lowest;

            return lowest * DIAGONAL_MOVE_COST + horizontalMovesRequired * DEFAULT_MOVE_COST;
        }

        public void SetG(float g) => G = g;
        public void SetH(float h) => H = h;
        public void SetPreviousTile(BaseTile previousTile) => PreviousTile = previousTile;

        public void PathHighlight() => _pathHighlight.SetActive(true);
        public void PathUnhighlight() => _pathHighlight.SetActive(false);

        public void MouseHighlight() => _mouseHighlight.SetActive(true);
        public void MouseUnhighlight() => _mouseHighlight.SetActive(false);

        public void AssignPointOfInterest(IPointOfInterest pointOfInterest)
        {
            _pointOfInterest = pointOfInterest;
            Walkable = false;
        }

        public void CacheNeighbours(TileGameObjectPlacer tileGameObjectPlacer)
        {
            List<Vector2Int> neighbourDirections = new()
            {
                new Vector2Int(0, 1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1)
            };

            Neighbours = new();

            foreach (var direction in neighbourDirections)
            {
                BaseTile neighbour = tileGameObjectPlacer.GetTileAtPosition(Origin + direction);

                if(neighbour != null)
                    Neighbours.Add(neighbour);
            }
        }

        public void RevealTile() => IsRevealed = true;

        private void OnMouseDown()
        {
            _pointOfInterest?.Interact();
        }

        private void OnMouseEnter()
        {
            MouseHighlight();
            OnMouseHover?.Invoke(this);
        }

        private void OnMouseExit()
        {
            MouseUnhighlight();
            _mouseHighlight.gameObject.SetActive(false);
        }
    }
}