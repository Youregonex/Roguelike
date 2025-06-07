using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yg.MapGeneration;
using Yg.YgPathFinder;
using System;
using Yg.FOW;

namespace Yg.PlayerCharacter
{
    public class Character : MonoBehaviour
    {
        public event Action OnTilePositionSnap;

        [CustomHeader("Settings")]
        [SerializeField] private float _moveSpeed;

        private TileGameObjectPlacer _tileGameObjectPlacer;

        private List<BaseTile> _currentPath = new();
        private Vector2Int _pressedTilePosition;

        private bool _isInitialized = false;
        private bool _isMoving = false;
        private Vector2 _currentMovementPoint;

        private Coroutine _currentMoveCoroutine;

        public void Initialize(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _tileGameObjectPlacer.OnTileHighlight += TileGameObjectPlacer_OnTileHighlight;
            _isInitialized = true;

            PlayerFogOfWar playerFogOfWar = GetComponentInChildren<PlayerFogOfWar>();
            playerFogOfWar.Initialize(this);
        }

        private void TileGameObjectPlacer_OnTileHighlight(BaseTile hoveredTile)
        {
            if (_isMoving) return;

            if(!hoveredTile.Walkable)
            {
                UnhighlightCurrentPath();
                return;
            }

            UnhighlightCurrentPath();
            _currentPath = Pathfinder.FindPath(GetCurrentTile(), hoveredTile);
            HighlightCurrentPath();
        }

        private void Update()
        {
            if (!_isInitialized || _isMoving) return;

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                ProcessMousePress();
            }
        }

        private void ProcessMousePress()
        {
            _pressedTilePosition = GetMouseSnapedPosition();
            BaseTile pressedTile = _tileGameObjectPlacer.GetTileAtPosition(_pressedTilePosition);
            
            if (pressedTile == null || !pressedTile.Walkable) return;

            _currentPath = Pathfinder.FindPath(GetCurrentTile(), pressedTile);

            if (_currentPath.Count <= 0) return;

            if (_currentMoveCoroutine != null)
                StopAllCoroutines();

            _currentMoveCoroutine = StartCoroutine(MoveAlongPath());
        }

        private Vector2Int GetMouseSnapedPosition()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y));
        }

        private IEnumerator MoveAlongPath()
        {
            _isMoving = true;

            UnhighlightCurrentPath();
            float tileProximitySnapThreshold = .1f;

            for (int i = 0; i < _currentPath.Count; i++)   
            {
                _currentMovementPoint = _currentPath[i].Origin;
                while (Vector2.Distance(transform.position, _currentPath[i].Origin) > tileProximitySnapThreshold)
                {
                    transform.position = Vector2.MoveTowards(transform.position, _currentPath[i].Origin, _moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = new(_currentPath[i].Origin.x, _currentPath[i].Origin.y);
                OnTilePositionSnap?.Invoke();
            }

            _currentPath.Clear();
            _currentMoveCoroutine = null;
            _isMoving = false;

            if (_tileGameObjectPlacer.GetTileAtPosition(GetMouseSnapedPosition()) == null) yield break;
            TileGameObjectPlacer_OnTileHighlight(_tileGameObjectPlacer.GetTileAtPosition(GetMouseSnapedPosition()));
        }

        private BaseTile GetCurrentTile()
        {
            Vector2Int currentPosition = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            return _tileGameObjectPlacer.GetTileAtPosition(currentPosition);
        }

        private void UnhighlightCurrentPath()
        {
            if (_currentPath.Count > 0)
                _tileGameObjectPlacer.UnhighlightTiles(_currentPath);
        }

        private void HighlightCurrentPath()
        {
            if (_currentPath.Count > 0)
                _tileGameObjectPlacer.HighlightTiles(_currentPath);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_currentMovementPoint, .3f);
        }
    }
}
