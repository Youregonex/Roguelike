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

        private HashSet<Vector2Int> _visitedTilesPositionSet = new();
        private HashSet<Vector2Int> _cachedVisionPositionSet = new();
        private HashSet<Vector2Int> _currentVisionPositionSet = new();

        [Inject]
        private void Construct(Tileplacer tileplacer)
        {
            _tileplacer = tileplacer;
        }

        public void Initialize(Character playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _playerCharacter.OnTilePositionSnap += PlayerCharacter_OnTilePositionSnap;
        }

        private void Awake()
        {
            UpdateCurrentVisionTiles();
            CacheVisionTiles();
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

            UpdateCurrentVisionTiles();

            HashSet<Vector2Int> tilesToHide = _cachedVisionPositionSet.Except(_currentVisionPositionSet).ToHashSet();

            foreach (var tilePosition in tilesToHide)
                _tileplacer.PlaceVisitedFOW(tilePosition);

            CacheVisionTiles();
        }

        private void RevealTilePosition(Vector2Int currentPosition)
        {
            if (!_visitedTilesPositionSet.Contains(currentPosition))
                _visitedTilesPositionSet.Add(currentPosition);

            _tileplacer?.RemoveFOW(currentPosition);
        }

        private void UpdateCurrentVisionTiles()
        {
            _currentVisionPositionSet.Clear();

            Vector2Int currentPosition;

            for (int x = -_visionRadius / 2; x <= _visionRadius / 2; x++)
                for (int y = -_visionRadius / 2; y <= _visionRadius / 2; y++)
                {
                    currentPosition = new Vector2Int((int)transform.position.x + x, (int)transform.position.y + y);
                    _currentVisionPositionSet.Add(currentPosition);
                }
        }

        private void CacheVisionTiles()
        {
            _cachedVisionPositionSet.Clear();

            Vector2Int currentPosition;

            for (int x = -_visionRadius / 2; x <= _visionRadius / 2; x++)
                for (int y = -_visionRadius / 2; y <= _visionRadius / 2; y++)
                {
                    currentPosition = new Vector2Int((int)transform.position.x + x, (int)transform.position.y + y);
                    _cachedVisionPositionSet.Add(currentPosition);
                }
        }

        private void OnValidate()
        {
            if (_visionRadius % 2 == 0) _visionRadius++;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            foreach (var position in _cachedVisionPositionSet)
                Gizmos.DrawSphere((Vector2)position, .4f);

            Gizmos.color = Color.magenta;
            foreach (var position in _currentVisionPositionSet)
                Gizmos.DrawSphere((Vector2)position, .3f);

        }
    }
}