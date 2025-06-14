using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Yg.MapGeneration;
using Zenject;

namespace Yg.Player.FOW
{
    public class PlayerFogOfWar : PlayerCharacterComponent
    {
        [CustomHeader("TEST SETTINGS")]
        [SerializeField] private int _visionRange;

        private Tileplacer _tileplacer;
        private PlayerCharacter _playerCharacter;
        private MapAssembler _mapAssembler;
        private TileGameObjectPlacer _tileGameObjectPlacer;

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

        public override void InitializeComponent(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _playerCharacter.OnTilePositionSnap += PlayerCharacter_OnTilePositionSnap;

            ComputeVisionOffsets();

            if (_revealedTilePositionSet != null && _revealedTilePositionSet.Count > 0)
                LoadFOW();
            
            UpdateFOW();
        }

        private void OnDestroy()
        {
            _playerCharacter.OnTilePositionSnap -= PlayerCharacter_OnTilePositionSnap;
        }

        public override void SaveComponent(PlayerSaveData playerSaveData)
        {
            playerSaveData.RevealedFOWSet = _revealedTilePositionSet;
        }

        public override void LoadComponent(PlayerSaveData playerSaveData)
        {
            _revealedTilePositionSet = playerSaveData.RevealedFOWSet;
        }

        public object CaptureState()
        {
            return _revealedTilePositionSet;
        }

        public void RestoreState(object data)
        {
            var saveData = data as HashSet<Vector2Int>
                ?? JsonConvert.DeserializeObject<HashSet<Vector2Int>>(JsonConvert.SerializeObject(data));

            if (saveData == null) Debug.LogError("Data is null");

            _revealedTilePositionSet = saveData;
        }

        private void ComputeVisionOffsets()
        {
            for (int x = -_visionRange / 2; x <= _visionRange / 2; x++)
                for (int y = -_visionRange / 2; y <= _visionRange / 2; y++)
                    _visionOffsetSet.Add(new Vector2Int(x, y));
        }

        private void PlayerCharacter_OnTilePositionSnap()
        {
            UpdateFOW();
        }

        private void UpdateFOW()
        {
            Vector2Int currentPosition;
            Vector2Int characterPosition = new((int)transform.position.x, (int)transform.position.y);
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

        private void OnValidate()
        {
            if (_visionRange % 2 == 0) _visionRange++;
        }
    }
}