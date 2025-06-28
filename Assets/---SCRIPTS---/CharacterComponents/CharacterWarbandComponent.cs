using System.Collections.Generic;
using UnityEngine;

namespace Yg.Character
{
    public class CharacterWarbandComponent : PlayerCharacterComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] private int _warbandSlotsMax = 5;

        private List<WarbandSlot> _warbandSlotList = new();

        public override void LoadComponent(PlayerSaveData playerSaveData)
        {
            
        }

        public override void SaveComponent(PlayerSaveData playerSaveData)
        {
            
        }

        public void AddUnit(WarbandSlot warbandSlot)
        {
            if (_warbandSlotList.Count < _warbandSlotsMax)
                _warbandSlotList.Add(warbandSlot);
        }

        public void RemoveUnit(WarbandSlot warbandSlot)
        {
            if (_warbandSlotList.Contains(warbandSlot))
                _warbandSlotList.Remove(warbandSlot);
        }
    }
}
