using UnityEngine;
using Yg.MapGeneration;
using Zenject;

namespace Yg.Character
{
    public class PlayerInteractComponent : CharacterComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _interactionRange = 1.5f;

        private TileGameObjectPlacer _tileGameObjectPlacer;
        private BaseTile _lastClickedTile;

        [Inject]
        private void Construct(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                ProcessMousePress();
        }

        private void ProcessMousePress()
        {
            _lastClickedTile = _tileGameObjectPlacer.GetTileAtPosition(Utilities.GetMouseSnapedPosition());
            if (_lastClickedTile is null) return;

            if (Vector2.Distance(_lastClickedTile.Origin, transform.position) <= _interactionRange && !Utilities.MouseOverUI())
                _lastClickedTile.InteractWithPointOfInterest(_characterCore as PlayerCore);
        }

        public override void LoadComponent(CharacterSaveData characterSaveData) {}
        public override void SaveComponent(CharacterSaveData characterSaveData) {}
    }
}
