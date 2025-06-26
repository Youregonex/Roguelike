using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yg.MapGeneration;
using Yg.UI;
using Yg.YgPathFinder;
using Zenject;

namespace Yg.Player
{
    public class PlayerMovementComponent : PlayerCharacterComponent
    {
        public event Action OnTilePositionSnap;
        public event Action OnMovementStart;
        public event Action OnMovementStop;

        [CustomHeader("Settings")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private int _maxMovesPerTurn;

        private TileGameObjectPlacer _tileGameObjectPlacer;
        private MovementUI _movementUI;

        private List<BaseTile> _currentPath = new();

        private Vector2Int _pressedTilePosition;
        private Vector2 _currentMovementPoint;
        private int _movesLeft;

        private bool _isInitialized = false;
        private bool _isMoving = false;
        private bool _stopFlag = false;

        private Coroutine _currentMoveCoroutine;

        public int MovesLeft => _movesLeft;
        public bool IsMoving => _isMoving;

        [Inject]
        private void Construct(TileGameObjectPlacer tileGameObjectPlacer, MovementUI movementUI)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _movementUI = movementUI;
        }

        public override void InitializeComponent(PlayerCore playerCore)
        {
            base.InitializeComponent(playerCore);

            if (!_componentLoaded)
                _movesLeft = _maxMovesPerTurn;


            _movementUI.Show();
            _movementUI.UpdateMoves(_movesLeft);
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            if (Input.GetKeyDown(KeyCode.Q))
                ResetMoves();

            if (Input.GetKeyDown(KeyCode.Mouse1) && _isMoving)
                _stopFlag = true;

            if (Input.GetKeyDown(KeyCode.Mouse0) && !_isMoving)
                ProcessMousePress();
        }

        public override void LoadComponent(PlayerSaveData playerSaveData)
        {
            _movesLeft = playerSaveData.MovesLeft;
            _componentLoaded = true;
        }

        public override void SaveComponent(PlayerSaveData playerSaveData)
        {
            playerSaveData.MovesLeft = _movesLeft;
        }

        private void ProcessMousePress()
        {
            _pressedTilePosition = Utilities.GetMouseSnapedPosition();
            BaseTile pressedTile = _tileGameObjectPlacer.GetTileAtPosition(_pressedTilePosition);

            if (pressedTile == null || !pressedTile.PlayerWalkable || _movesLeft <= 0) return;

            _currentPath = Pathfinder.FindPath(_playerCore.GetCurrentTile(), pressedTile, true, _movesLeft);

            if (_currentPath.Count <= 0) return;
            if (_currentMoveCoroutine != null)
                StopAllCoroutines();

            _currentMoveCoroutine = StartCoroutine(MoveAlongPath());
        }

        private void ResetMoves()
        {
            _movesLeft = _maxMovesPerTurn;
            _movementUI.UpdateMoves(_movesLeft);
        }

        private IEnumerator MoveAlongPath()
        {
            _isMoving = true;
            float tileProximitySnapThreshold = .1f;
            OnMovementStart?.Invoke();

            for (int i = 0; i < _currentPath.Count; i++)
            {
                _currentMovementPoint = _currentPath[i].Origin;
                while (Vector2.Distance(transform.position, _currentPath[i].Origin) > tileProximitySnapThreshold)
                {
                    transform.root.position = Vector2.MoveTowards(transform.position, _currentPath[i].Origin, _moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.root.position = new(_currentPath[i].Origin.x, _currentPath[i].Origin.y);
                OnTilePositionSnap?.Invoke();
                _movesLeft--;
                _movementUI.UpdateMoves(_movesLeft);

                if (_movesLeft <= 0) break;

                if (_stopFlag)
                {
                    _stopFlag = false;
                    break;
                }
            }

            StopMovement();
        }

        private void StopMovement()
        {
            _currentPath.Clear();
            _currentMoveCoroutine = null;
            _isMoving = false;

            OnMovementStop?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_currentMovementPoint, .3f);
        }
    }
}
