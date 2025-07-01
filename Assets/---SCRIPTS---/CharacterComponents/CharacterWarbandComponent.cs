using System.Collections.Generic;
using UnityEngine;
using Yg.GameData.Units;

namespace Yg.Character
{
    public class CharacterWarbandComponent : CharacterComponent
    {
        [CustomHeader("Settings")]
        [SerializeField] protected int _warbandSize = 5;

        [CustomHeader("Debug")]
        [SerializeField] protected List<WarbandSlot> _warbandSlotList = new();

        public IEnumerable<WarbandSlot> Warband => _warbandSlotList;

        public override void InitializeComponent(CharacterCore characterCore)
        {
            base.InitializeComponent(characterCore);

            if (_warbandSlotList.Count <= 0)
                InitializeWarband();
        }

        public override void LoadComponent(CharacterSaveData characterSaveData)
        {
            _warbandSize = characterSaveData.WarbandSize;

            InitializeWarband();

            for (int i = 0; i < characterSaveData.WarbandSlotSaveDataList.Count; i++)
            {
                if (characterSaveData.WarbandSlotSaveDataList[i].Empty) continue;

                UnitDataSO unitDataSO = ResourceLoader.GetUnitDataSO(characterSaveData.WarbandSlotSaveDataList[i].PrefabId);
                WarbandSlot warbandSlot = new(unitDataSO, characterSaveData.WarbandSlotSaveDataList[i].SlotSize);

                AddSquad(warbandSlot);
            }
        }

        public override void SaveComponent(CharacterSaveData characterSaveData)
        {
            List<WarbandSlotSaveData> warbandSlotSaveDataList = new();

            for (int i = 0; i < _warbandSlotList.Count; i++)
            {
                WarbandSlotSaveData warbandSlotSaveData = new(_warbandSlotList[i]);
                warbandSlotSaveDataList.Add(warbandSlotSaveData);
            }

            characterSaveData.WarbandSlotSaveDataList = warbandSlotSaveDataList;
            characterSaveData.WarbandSize = _warbandSize;
        }

        public virtual void AddWarbandSlot()
        {
            if (_warbandSlotList.Count < _warbandSize)
                _warbandSlotList.Add(new WarbandSlot());
        }

        public virtual void RemoveWarbandSlot()
        {
            if (_warbandSlotList.Count > 0)
                _warbandSlotList.RemoveAt(0);
        }

        public virtual bool AddSquad(WarbandSlot warbandSlot)
        {
            if(warbandSlot.Empty)
            {
                Debug.LogError("Trying to add empty squad!");
                return false;
            }

            for (int i = 0; i < _warbandSlotList.Count; i++)
            {
                if (_warbandSlotList[i].Empty)
                {
                    _warbandSlotList[i].UpdateData(warbandSlot.UnitData, warbandSlot.SlotSize);
                    return true;
                }
            }

            return false;
        }

        public virtual void RemoveSquad(WarbandSlot warbandSlot)
        {
            warbandSlot.ClearSlot();
        }

        private void InitializeWarband()
        {
            for (int i = 0; i < _warbandSize; i++)
                AddWarbandSlot();
        }
    }
}
