using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.MapGeneration;
using Yg.PlayerCharacter;
using Zenject;

namespace Yg.FOW
{
    public class PlayerFogOfWar : MonoBehaviour
    {
        [CustomHeader("TEST SETTINGS")]
        [SerializeField] private int _visionRadius;

        private Tileplacer _tileplacer;
        private Character _playerCharacter;
        private MapAssembler _mapAssembler;
        private TileGameObjectPlacer _tileGameObjectPlacer;

        private HashSet<Vector2Int> _visitedTilesPositionSet = new();
        private HashSet<Vector2Int> _cachedVisionPositionSet = new();
        private HashSet<Vector2Int> _currentVisionPositionSet = new();

        [Inject]
        private void Construct(Tileplacer tileplacer, MapAssembler mapAssembler, TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileplacer = tileplacer;
            _mapAssembler = mapAssembler;
            _tileGameObjectPlacer = tileGameObjectPlacer;
        }

        public void Initialize(Character playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _playerCharacter.OnTilePositionSnap += PlayerCharacter_OnTilePositionSnap;
        }

        private void Awake()
        {
            UpdateVisionTiles(_currentVisionPositionSet);
            UpdateVisionTiles(_cachedVisionPositionSet);
            UpdateFOW();
        }

        private void OnDestroy()
        {
            _playerCharacter.OnTilePositionSnap -= PlayerCharacter_OnTilePositionSnap;
        }

        private void PlayerCharacter_OnTilePositionSnap()
        {
            UpdateFOW();
        }

        private void UpdateFOW()
        {
            Vector2Int currentPosition;

            for (int x = -_visionRadius / 2; x <= _visionRadius / 2; x++)
                for (int y = -_visionRadius / 2; y <= _visionRadius / 2; y++)
                {
                    currentPosition = new Vector2Int((int)transform.position.x + x, (int)transform.position.y + y);
                    RevealTilePosition(currentPosition);
                }

            UpdateVisionTiles(_currentVisionPositionSet);

            HashSet<Vector2Int> tilesToHide = _cachedVisionPositionSet.Except(_currentVisionPositionSet).ToHashSet();

            foreach (var tilePosition in tilesToHide)
                _tileplacer.PlaceVisitedFOW(tilePosition);

            UpdateVisionTiles(_cachedVisionPositionSet);
        }

        private void RevealTilePosition(Vector2Int currentPosition)
        {
            if (!_visitedTilesPositionSet.Contains(currentPosition))
                _visitedTilesPositionSet.Add(currentPosition);

            _tileGameObjectPlacer.RevealTileAt(currentPosition);
            _tileplacer?.RemoveFOW(currentPosition);
        }

        private void UpdateVisionTiles(HashSet<Vector2Int> visionTilesSet)
        {
            visionTilesSet.Clear();

            Vector2Int currentPosition;

            for (int x = -_visionRadius / 2; x <= _visionRadius / 2; x++)
                for (int y = -_visionRadius / 2; y <= _visionRadius / 2; y++)
                {
                    currentPosition = new Vector2Int((int)transform.position.x + x, (int)transform.position.y + y);

                    if(_mapAssembler.WithinBounds(currentPosition))
                        visionTilesSet.Add(currentPosition);
                }
        }

        private void OnValidate()
        {
            if (_visionRadius % 2 == 0) _visionRadius++;
        }
    }
}