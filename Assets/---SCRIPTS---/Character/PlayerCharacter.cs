using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yg.MapGeneration;
using Yg.YgPathFinder;
using System;
using Yg.Player.FOW;
using Zenject;

namespace Yg.Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        public event Action OnTilePositionSnap;

        [CustomHeader("Settings")]
        [SerializeField] private float _moveSpeed;

        private TileGameObjectPlacer _tileGameObjectPlacer;

        private List<BaseTile> _currentPath = new();
        private List<PlayerCharacterComponent> _playerCharacterComponentList = new();
        private Vector2Int _pressedTilePosition;
        private Vector2 _currentMovementPoint;
        private bool _isInitialized = false;
        private bool _isMoving = false;
        private bool _stopFlag = false;

        private Coroutine _currentMoveCoroutine;

        [Inject]
        private void Construct(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _tileGameObjectPlacer.OnTileHighlight += TileGameObjectPlacer_OnTileHighlight;
        }

        public void Initialize(PlayerSaveData playerSaveData)
        {
            GatherPlayerCharacterComponents();

            if (playerSaveData != null)
                LoadPlayerState(playerSaveData);

            InitializePlayerCharacterComponents();

            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            if (Input.GetKeyDown(KeyCode.Mouse1) && _isMoving)
                _stopFlag = true;
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && !_isMoving)
                ProcessMousePress();
        }

        private void OnDestroy()
        {
            _tileGameObjectPlacer.OnTileHighlight -= TileGameObjectPlacer_OnTileHighlight;
        }

        public PlayerSaveData SavePlayerState()
        {
            PlayerSaveData playerSaveData = new();
            playerSaveData.Position = Vector2Int.RoundToInt(transform.position);

            foreach (var component in _playerCharacterComponentList)
                component.SaveComponent(playerSaveData);

            return playerSaveData;
        }

        private void LoadPlayerState(PlayerSaveData playerSaveData)
        {
            transform.position = (Vector2)playerSaveData.Position;
            foreach (var component in _playerCharacterComponentList)
                component.LoadComponent(playerSaveData);
        }

        private void GatherPlayerCharacterComponents()
        {
            foreach (var component in GetComponentsInChildren<PlayerCharacterComponent>())
                _playerCharacterComponentList.Add(component);
        }

        private void InitializePlayerCharacterComponents()
        {
            foreach (var component in _playerCharacterComponentList)
                component.InitializeComponent(this);
        }

        private void ProcessMousePress()
        {
            _pressedTilePosition = GetMouseSnapedPosition();
            BaseTile pressedTile = _tileGameObjectPlacer.GetTileAtPosition(_pressedTilePosition);
            
            if (pressedTile == null || !pressedTile.PlayerWalkable) return;

            _currentPath = Pathfinder.FindPath(GetCurrentTile(), pressedTile, true);

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

                if(_stopFlag)
                {
                    _stopFlag = false;
                    break;
                }
            }

            _currentPath.Clear();
            _currentMoveCoroutine = null;
            _isMoving = false;

            if (_tileGameObjectPlacer.GetTileAtPosition(GetMouseSnapedPosition()) == null) yield break;
            TileGameObjectPlacer_OnTileHighlight(_tileGameObjectPlacer.GetTileAtPosition(GetMouseSnapedPosition()));
        }

        private void TileGameObjectPlacer_OnTileHighlight(BaseTile hoveredTile)
        {
            if (_isMoving) return;

            if (!hoveredTile.PlayerWalkable)
            {
                UnhighlightCurrentPath();
                return;
            }

            UnhighlightCurrentPath();
            _currentPath = Pathfinder.FindPath(GetCurrentTile(), hoveredTile, true);
            HighlightCurrentPath();
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

    public class PlayerSaveData
    {
        public Vector2Int Position;
        public HashSet<Vector2Int> RevealedFOWSet;
    }
}