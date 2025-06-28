using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.MapGeneration;
using Zenject;

namespace Yg.Character
{
    public class PlayerCore : MonoBehaviour
    {
        private TileGameObjectPlacer _tileGameObjectPlacer;

        private readonly HashSet<PlayerCharacterComponent> _playerCharacterComponentList = new();

        [Inject]
        private void Costruct(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
        }

        public void Initialize(PlayerSaveData playerSaveData)
        {
            GatherPlayerCharacterComponents();

            if (playerSaveData is not null)
                LoadPlayerState(playerSaveData);

            InitializePlayerCharacterComponents();
        }

        public T GetPlayerComponent<T>() where T : PlayerCharacterComponent
        {
            return _playerCharacterComponentList.OfType<T>().FirstOrDefault();
        }

        public PlayerSaveData SavePlayerState()
        {
            PlayerSaveData playerSaveData = new();
            playerSaveData.Position = Vector2Int.RoundToInt(transform.position);

            foreach (var component in _playerCharacterComponentList)
                component.SaveComponent(playerSaveData);

            return playerSaveData;
        }

        public BaseTile GetCurrentTile()
        {
            Vector2Int currentPosition = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            return _tileGameObjectPlacer.GetTileAtPosition(currentPosition);
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
    }

    public class PlayerSaveData
    {
        public Vector2Int Position;
        public HashSet<Vector2Int> RevealedFOWSet;
        public int MovesLeft;
    }
}