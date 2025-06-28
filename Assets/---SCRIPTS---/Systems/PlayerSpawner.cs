using Newtonsoft.Json;
using UnityEngine;
using Yg.SaveLoad;
using Zenject;

namespace Yg.Character
{
    public class PlayerSpawner : MonoBehaviour, ISaveable
    {
        private PlayerCore _characterPrefab;
        private PlayerCore _character;

        private PlayerSaveData _playerSaveData = null;
        private DiContainer _container;

        public PlayerCore Character => _character;

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
            _character = _container.InstantiatePrefab(_characterPrefab, Vector2.zero, Quaternion.identity, null).GetComponent<PlayerCore>();
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
