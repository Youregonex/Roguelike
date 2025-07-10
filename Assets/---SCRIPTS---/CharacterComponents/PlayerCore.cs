using System.Collections.Generic;
using UnityEngine;
using Yg.GameData.Units;
using Yg.Systems;
using Zenject;

namespace Yg.Character
{
    public class PlayerCore : CharacterCore
    {
        private BattleInitiator _battleInitiator;
        private UnitSelectionGenerator _unitSelectionGenerator;

        [Inject]
        private void Construct(BattleInitiator battleInitiator)
        {
            _battleInitiator = battleInitiator;
        }

        public override void Initialize(CharacterSaveData characterSaveData)
        {
            base.Initialize(characterSaveData);
            _unitSelectionGenerator = new();
        }

        public override CharacterSaveData SaveCharacterState()
        {
            PlayerSaveData playerSaveData = new();
            playerSaveData.CharacterSaveDataType = ECharacterSaveDataType.Player;

            playerSaveData.Position = Vector2Int.RoundToInt(transform.position);

            foreach (var component in _characterComponentSet)
                component.SaveComponent(playerSaveData);

            return playerSaveData;
        }

        public void EncounterBattle()
        {
            int maxSlots = 6;
            List<WarbandSlot> enemyWarband = new();
            List<UnitDataSO> enemyUnits = _unitSelectionGenerator.GenerateRandomUnitChoiceList(maxSlots);

            for (int i = 0; i < enemyUnits.Count; i++)
            {
                WarbandSlot warbandSlot = new(enemyUnits[i]);
                enemyWarband.Add(warbandSlot);
            }

            List<WarbandSlot> playerWarband = new(GetCharacterComponent<PlayerWarbandComponent>().Warband);
            _battleInitiator.StartBattle(playerWarband, enemyWarband);
        }

        protected override void LoadCharacterState(CharacterSaveData characterSaveData)
        {
            if(characterSaveData is not PlayerSaveData)
            {
                Debug.LogError("Player core recieved CharacterSaveData instead of PlayerSaveData");
                return;
            }

            PlayerSaveData playerSaveData = characterSaveData as PlayerSaveData;
            transform.position = (Vector2)characterSaveData.Position;
            foreach (var component in _characterComponentSet)
                component.LoadComponent(playerSaveData);
        }
    }

    public class PlayerSaveData : CharacterSaveData
    {
        public HashSet<Vector2Int> RevealedFOWSet;
        public int MovesLeft;

        public void DebugData()
        {
            Debug.Log($"PlayerSaveData:\nType {CharacterSaveDataType}\nPosition {Position}\nWarbandSize {WarbandSize}\nCount{WarbandSlotSaveDataList.Count}\nRevealed {RevealedFOWSet.Count}\nMovesLeft{MovesLeft}");
        }
    }
}
