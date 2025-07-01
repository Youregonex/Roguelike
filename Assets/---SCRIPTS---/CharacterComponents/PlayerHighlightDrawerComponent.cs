using System.Collections.Generic;
using UnityEngine;
using Yg.MapGeneration;
using Yg.YgPathFinder;
using Zenject;

namespace Yg.Character
{
    public class PlayerHighlightDrawerComponent : CharacterComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private GameObject _mouseGameObject;

        private TileGameObjectPlacer _tileGameObjectPlacer;
        private CharacterMovementComponent _movementComponent;

        private List<BaseTile> _currentPath = new();

        [Inject]
        private void Construct(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
            _tileGameObjectPlacer.OnTileHighlight += TileGameObjectPlacer_OnTileHighlight;
        }

        public override void InitializeComponent(CharacterCore playerCore)
        {
            base.InitializeComponent(playerCore);

            _movementComponent = _characterCore.GetCharacterComponent<CharacterMovementComponent>();
            _movementComponent.OnMovementStart += MovementComponent_OnMovementStart;
            _movementComponent.OnMovementStop += MovementComponent_OnMovementStop;
        }

        private void MovementComponent_OnMovementStop()
        {
            BaseTile hoveredTile = _tileGameObjectPlacer.GetTileAtPosition(Utilities.GetMouseSnapedPosition());

            _mouseGameObject.SetActive(true);

            if (hoveredTile is not null)
                TileGameObjectPlacer_OnTileHighlight(hoveredTile);
        }

        private void MovementComponent_OnMovementStart()
        {
            _mouseGameObject.SetActive(false);
            UnhighlightCurrentPath();
        }

        public override void LoadComponent(CharacterSaveData playerSaveData) { }
        public override void SaveComponent(CharacterSaveData playerSaveData) { }

        private void TileGameObjectPlacer_OnTileHighlight(BaseTile hoveredTile)
        {
            UnhighlightCurrentPath();
            _mouseGameObject.transform.position = (Vector2)hoveredTile.Origin;

            if (Utilities.MouseOverUI()) return;
            if (_movementComponent.IsMoving) return;
            if (!hoveredTile.PlayerWalkable || _movementComponent.MovesLeft <= 0) return;

            _currentPath = Pathfinder.FindPath(_characterCore.GetCurrentTile(), hoveredTile, true, _movementComponent.MovesLeft);
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
