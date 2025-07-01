using System.Collections.Generic;
using UnityEngine;
using Yg.MapGeneration;
using Zenject;

namespace Yg.Character.FOW
{
    public class PlayerFogOfWar : CharacterComponent
    {
        [CustomHeader("TEST SETTINGS")]
        [SerializeField] private int _visionRange;

        private Tileplacer _tileplacer;
        private MapAssembler _mapAssembler;
        private TileGameObjectPlacer _tileGameObjectPlacer;
        private CharacterMovementComponent _playerMovementComponent;

        private HashSet<Vector2Int> _revealedTilePositionSet = new();

        private readonly HashSet<Vector2Int> _visionOffsetSet = new();
        private readonly HashSet<Vector2Int> _cachedVisionSet = new();
        private readonly HashSet<Vector2Int> _currentVisionSet = new();

        [Inject]
        private void Construct(Tileplacer tileplacer, MapAssembler mapAssembler, TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileplacer = tileplacer;
            _mapAssembler = mapAssembler;
            _tileGameObjectPlacer = tileGameObjectPlacer;
        }

        public override void InitializeComponent(CharacterCore playerCharacter)
        {
            _playerMovementComponent = transform.root.GetComponent<CharacterCore>().GetCharacterComponent<CharacterMovementComponent>();
            _playerMovementComponent.OnTilePositionSnap += PlayerCharacter_OnTilePositionSnap;

            ComputeVisionOffsets();

            if (_revealedTilePositionSet != null && _revealedTilePositionSet.Count > 0)
                LoadFOW();
            
            UpdateFOW();
        }

        private void OnDestroy()
        {
            _playerMovementComponent.OnTilePositionSnap -= PlayerCharacter_OnTilePositionSnap;
        }

        public override void SaveComponent(CharacterSaveData characterSaveData)
        {
            if (characterSaveData is not PlayerSaveData)
            {
                Debug.LogError("Wrong save data type!");
                return;
            }

            PlayerSaveData playerSaveData = characterSaveData as PlayerSaveData;
            playerSaveData.RevealedFOWSet = _revealedTilePositionSet;
        }

        public override void LoadComponent(CharacterSaveData characterSaveData)
        {
            if (characterSaveData is not PlayerSaveData)
            {
                Debug.LogError("Wrong save data type!");
                return;
            }

            PlayerSaveData playerSaveData = characterSaveData as PlayerSaveData;
            _revealedTilePositionSet = playerSaveData.RevealedFOWSet;
        }

        private void ComputeVisionOffsets()
        {
            for (int x = -_visionRange; x <= _visionRange; x++)
            {
                for (int y = -_visionRange; y <= _visionRange; y++)
                {
                    if (Mathf.Abs(x) == _visionRange || Mathf.Abs(y) == _visionRange) continue;

                    if (x * x + y * y <= _visionRange * _visionRange)
                    {
                        _visionOffsetSet.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        private void PlayerCharacter_OnTilePositionSnap()
        {
            UpdateFOW();
        }

        private void UpdateFOW()
        {
            Vector2Int currentPosition;
            Vector2Int characterPosition = Vector2Int.RoundToInt(transform.position);
            _currentVisionSet.Clear();

            foreach (var visionOffset in _visionOffsetSet)
            {
                currentPosition = visionOffset + characterPosition;

                if(_mapAssembler.WithinBounds(currentPosition))
                {
                    _currentVisionSet.Add(currentPosition);

                    if (!_revealedTilePositionSet.Contains(currentPosition))
                        _revealedTilePositionSet.Add(currentPosition);

                    RevealTilePosition(currentPosition);
                }
            }

            foreach (var tilePosition in _cachedVisionSet)
            {
                if (!_currentVisionSet.Contains(tilePosition))
                    _tileplacer.PlaceVisitedFOW(tilePosition);
            }

            _cachedVisionSet.Clear();
            _cachedVisionSet.UnionWith(_currentVisionSet);
        }

        private void RevealTilePosition(Vector2Int currentPosition)
        {
            _tileGameObjectPlacer.RevealTileAt(currentPosition);
            _tileplacer?.RemoveFOW(currentPosition);
        }

        private void LoadFOW()
        {
            foreach (var position in _revealedTilePositionSet)
            {
                _tileGameObjectPlacer.RevealTileAt(position);
                _tileplacer?.PlaceVisitedFOW(position);
            }
        }
    }
}