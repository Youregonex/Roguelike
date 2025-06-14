using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Yg.SaveLoad;
using Zenject;

namespace Yg.Player
{
    public class PlayerSpawner : MonoBehaviour, ISaveable
    {
        private PlayerCharacter _characterPrefab;
        private PlayerCharacter _character;

        private PlayerSaveData _playerSaveData = null;
        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _characterPrefab = ResourceLoader.PREFAB_PlayerCharacter;
        }

        public void SpawnPlayer()
        {
            _character = _container.InstantiatePrefab(_characterPrefab, Vector2.zero, Quaternion.identity, null).GetComponent<PlayerCharacter>();
            _character.Initialize(_playerSaveData);
        }

        public object CaptureState()
        {
            PlayerSaveData playerSaveData = _character.SavePlayerState();

            return playerSaveData;
        }

        public void RestoreState(object data)
        {
            var playerData = data as PlayerSaveData
                ?? JsonConvert.DeserializeObject<PlayerSaveData>(JsonConvert.SerializeObject(data));

            if (playerData == null) Debug.LogError("Data is null");

            _playerSaveData = playerData;
        }
    }
}
