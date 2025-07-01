using System.Collections;
using UnityEngine;
using Yg.MapGeneration;
using Yg.UI;
using Yg.YgPathFinder;
using Zenject;

namespace Yg.Character
{
    public class PlayerMovementComponent : CharacterMovementComponent
    {
        private MovementUI _movementUI;
        private bool _stopFlag = false;
        private Vector2Int _pressedTilePosition;

        private bool _movementLocked = false;

        [Inject]
        protected void Construct(MovementUI movementUI)
        {
            _movementUI = movementUI;
        }

        public override void InitializeComponent(CharacterCore characterCore)
        {
            base.InitializeComponent(characterCore);

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

        public override void LoadComponent(CharacterSaveData characterSaveData)
        {
            if(characterSaveData is not PlayerSaveData)
            {
                Debug.LogError("Wrong save data type!");
                return;
            }

            PlayerSaveData playerSaveData = characterSaveData as PlayerSaveData;

            _movesLeft = playerSaveData.MovesLeft;
            _componentLoaded = true;
        }

        public override void SaveComponent(CharacterSaveData characterSaveData)
        {
            if (characterSaveData is not PlayerSaveData)
            {
                Debug.LogError("Wrong save data type!");
                return;
            }

            PlayerSaveData playerSaveData = characterSaveData as PlayerSaveData;
            playerSaveData.MovesLeft = _movesLeft;
        }

        public void LockMovement() => _movementLocked = true;
        public void UnlockMovement() => _movementLocked = false;

        protected override IEnumerator MoveAlongPath()
        {
            _isMoving = true;
            float tileProximitySnapThreshold = .1f;

            InvokeOnMovementStart();

            for (int i = 0; i < _currentPath.Count; i++)
            {
                _currentMovementPoint = _currentPath[i].Origin;
                while (Vector2.Distance(transform.position, _currentPath[i].Origin) > tileProximitySnapThreshold)
                {
                    transform.root.position = Vector2.MoveTowards(transform.position, _currentPath[i].Origin, _moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.root.position = new(_currentPath[i].Origin.x, _currentPath[i].Origin.y);

                InvokeOnTilePositionSnap();
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

        protected override void ResetMoves()
        {
            base.ResetMoves();
            _movementUI.UpdateMoves(_movesLeft);
        }

        private void ProcessMousePress()
        {
            if (Utilities.MouseOverUI() || _movementLocked) return;

            _pressedTilePosition = Utilities.GetMouseSnapedPosition();
            BaseTile pressedTile = _tileGameObjectPlacer.GetTileAtPosition(_pressedTilePosition);

            if (pressedTile == null || !pressedTile.PlayerWalkable || _movesLeft <= 0) return;

            _currentPath = Pathfinder.FindPath(_characterCore.GetCurrentTile(), pressedTile, true, _movesLeft);

            if (_currentPath.Count <= 0) return;
            if (_currentMoveCoroutine != null)
                StopAllCoroutines();

            _currentMoveCoroutine = StartCoroutine(MoveAlongPath());
        }
    }
}
