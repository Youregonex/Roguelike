using System.Collections.Generic;
using UnityEngine;
using Yg.MapGeneration;
using Yg.YgPathFinder;
using Zenject;

namespace Yg.Player
{
    public class PlayerHighlightDrawerComponent : PlayerCharacterComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private GameObject _mouseGameObject;

        private TileGameObjectPlacer _tileGameObjectPlacer;
        private PlayerMovementComponent _movementComponent;

        private List<BaseTile> _currentPath = new();

        [Inject]
        private void Construct(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _tileGameObjectPlacer.OnTileHighlight += TileGameObjectPlacer_OnTileHighlight;
        }

        public override void InitializeComponent(PlayerCore playerCore)
        {
            base.InitializeComponent(playerCore);

            _movementComponent = _playerCore.GetPlayerComponent<PlayerMovementComponent>();
            _movementComponent.OnMovementStart += MovementComponent_OnMovementStart;
            _movementComponent.OnMovementStop += MovementComponent_OnMovementStop;
        }

        private void MovementComponent_OnMovementStop()
        {
            BaseTile hoveredTile = _tileGameObjectPlacer.GetTileAtPosition(Utilities.GetMouseSnapedPosition());
            TileGameObjectPlacer_OnTileHighlight(hoveredTile);
            _mouseGameObject.SetActive(true);
        }

        private void MovementComponent_OnMovementStart()
        {
            _mouseGameObject.SetActive(false);
            UnhighlightCurrentPath();
        }

        public override void LoadComponent(PlayerSaveData playerSaveData) { }
        public override void SaveComponent(PlayerSaveData playerSaveData) { }

        private void TileGameObjectPlacer_OnTileHighlight(BaseTile hoveredTile)
        {
            _mouseGameObject.transform.position = (Vector2)hoveredTile.Origin;

            if (_movementComponent.IsMoving) return;

            UnhighlightCurrentPath();

            if (!hoveredTile.PlayerWalkable || _movementComponent.MovesLeft <= 0) return;

            _currentPath = Pathfinder.FindPath(_playerCore.GetCurrentTile(), hoveredTile, true, _movementComponent.MovesLeft);
            HighlightCurrentPath();
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
    }
}
