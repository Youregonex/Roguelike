using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yg.MapGeneration;
using Yg.YgPathFinder;
using Zenject;

namespace Yg.Character
{
    public class CharacterMovementComponent : CharacterComponent
    {
        public event Action OnTilePositionSnap;
        public event Action OnMovementStart;
        public event Action OnMovementStop;

        [CustomHeader("Settings")]
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected int _maxMovesPerTurn;

        protected TileGameObjectPlacer _tileGameObjectPlacer;

        protected List<BaseTile> _currentPath = new();

        protected Vector2 _currentMovementPoint;
        protected int _movesLeft;

        protected bool _isInitialized = false;
        protected bool _isMoving = false;

        protected Coroutine _currentMoveCoroutine;

        public int MovesLeft => _movesLeft;
        public bool IsMoving => _isMoving;

        [Inject]
        protected void Construct(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
        }

        public override void InitializeComponent(CharacterCore characterCore)
        {
            base.InitializeComponent(characterCore);

            _movesLeft = _maxMovesPerTurn;
            _isInitialized = true;
        }

        public override void LoadComponent(CharacterSaveData characterSaveData) {}
        public override void SaveComponent(CharacterSaveData characterSaveData) {}

        protected virtual void ResetMoves()
        {
            _movesLeft = _maxMovesPerTurn;
        }

        protected virtual IEnumerator MoveAlongPath()
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

                if (_movesLeft <= 0) break;
            }

            StopMovement();
        }

        protected void InvokeOnTilePositionSnap() => OnTilePositionSnap?.Invoke();
        protected void InvokeOnMovementStart() => OnMovementStart?.Invoke();

        protected void StopMovement()
        {
            _currentPath.Clear();
            _currentMoveCoroutine = null;
            _isMoving = false;

            OnMovementStop?.Invoke();
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_currentMovementPoint, .3f);
        }
    }
}
