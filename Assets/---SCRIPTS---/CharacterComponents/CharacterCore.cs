using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Yg.Converters;
using Yg.MapGeneration;
using Zenject;

namespace Yg.Character
{
    public class CharacterCore : MonoBehaviour
    {
        protected TileGameObjectPlacer _tileGameObjectPlacer;

        protected readonly HashSet<CharacterComponent> _characterComponentSet = new();

        [Inject]
        private void Costruct(TileGameObjectPlacer tileGameObjectPlacer)
        {
            _tileGameObjectPlacer = tileGameObjectPlacer;
        }

        public virtual void Initialize(CharacterSaveData characterSaveData)
        {
            GatherCharacterComponents();

            if (characterSaveData is not null)
                LoadCharacterState(characterSaveData);

            InitializeCharacterComponents();
        }

        public T GetCharacterComponent<T>() where T : CharacterComponent
        {
            return _characterComponentSet.OfType<T>().FirstOrDefault();
        }

        public virtual CharacterSaveData SaveCharacterState()
        {
            CharacterSaveData characterSaveData = new();
            characterSaveData.CharacterSaveDataType = ECharacterSaveDataType.Default;

            characterSaveData.Position = Vector2Int.RoundToInt(transform.position);

            foreach (var component in _characterComponentSet)
                component.SaveComponent(characterSaveData);

            return characterSaveData;
        }

        protected virtual void LoadCharacterState(CharacterSaveData playerSaveData)
        {
            transform.position = (Vector2)playerSaveData.Position;
            foreach (var component in _characterComponentSet)
                component.LoadComponent(playerSaveData);
        }

        public BaseTile GetCurrentTile()
        {
            Vector2Int currentPosition = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            return _tileGameObjectPlacer.GetTileAtPosition(currentPosition);
        }

        private void GatherCharacterComponents()
        {
            foreach (var component in GetComponentsInChildren<CharacterComponent>())
                _characterComponentSet.Add(component);
        }

        private void InitializeCharacterComponents()
        {
            foreach (var component in _characterComponentSet)
                component.InitializeComponent(this);
        }
    }

    [JsonConverter(typeof(CharacterSaveDataConverter))]
    public class CharacterSaveData
    {
        public ECharacterSaveDataType CharacterSaveDataType;
        public Vector2Int Position;
        public int WarbandSize;
        public List<WarbandSlotSaveData> WarbandSlotSaveDataList;
    }

    public enum ECharacterSaveDataType
    {
        Default,
        Player
    }
}