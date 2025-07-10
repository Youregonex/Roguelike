using Newtonsoft.Json;
using UnityEngine;
using Yg.SaveLoad;
using Zenject;

namespace Yg.Character
{
    [SelectionBase]
    public class PlayerSpawner : MonoBehaviour, ISaveable
    {
        private PlayerCore _characterPrefab;
        private PlayerCore _playerCore;

        private CharacterSaveData _playerSaveData = null;
        private DiContainer _container;

        public PlayerCore PlayerCore => _playerCore;

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
            _playerCore = _container.InstantiatePrefab(_characterPrefab, Vector2.zero, Quaternion.identity, null).GetComponent<PlayerCore>();
            _playerCore.Initialize(_playerSaveData);
        }

        public object CaptureState()
        {
            CharacterSaveData playerSaveData = _playerCore.SaveCharacterState();

            return playerSaveData;
        }

        public void RestoreState(object data)
        {
            var playerData = data as CharacterSaveData
                ?? JsonConvert.DeserializeObject<CharacterSaveData>(JsonConvert.SerializeObject(data));

            if (playerData is null)
            {
                Debug.LogError("Data is null");
                return;
            }

            _playerSaveData = playerData;
        }
    }
}
