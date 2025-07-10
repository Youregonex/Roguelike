using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.GameData.Equipment;
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
        public bool HasEmptySlot => _warbandSlotList.Where(e => e.UnitEmpty).Any();

        public override void InitializeComponent(CharacterCore characterCore)
        {
            base.InitializeComponent(characterCore);

            if (_warbandSlotList.Count <= 0)
                InitializeWarband();
        }

        public override void LoadComponent(CharacterSaveData characterSaveData)
        {
            _warbandSize = characterSaveData.WarbandSize;

            for (int i = 0; i < characterSaveData.WarbandSlotSaveDataList.Count; i++)
            {
                WarbandSlot warbandSlot = new();

                for (int j = 0; j < characterSaveData.WarbandSlotSaveDataList[i].EquipmentDataList.Count; j++)
                {
                    if (characterSaveData.WarbandSlotSaveDataList[i].EquipmentDataList[j].IsEmpty) continue;
                    warbandSlot.AddEquipmentData(characterSaveData.WarbandSlotSaveDataList[i].EquipmentDataList[j]);
                }

                if (!characterSaveData.WarbandSlotSaveDataList[i].Empty)
                {
                    UnitDataSO unitDataSO = ResourceLoader.GetUnitDataSO(characterSaveData.WarbandSlotSaveDataList[i].PrefabId);
                    warbandSlot.UpdateUnitData(unitDataSO);
                }

                AddWarbandSlot(warbandSlot);
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

        public virtual void AddEmptyWarbandSlot()
        {
            if (_warbandSlotList.Count < _warbandSize)
                _warbandSlotList.Add(new WarbandSlot());
        }

        public virtual void RemoveWarbandSlot()
        {
            if (_warbandSlotList.Count > 0)
                _warbandSlotList.RemoveAt(0);
        }

        public virtual void AddWarbandSlot(WarbandSlot warbandSlot)
        {
            _warbandSlotList.Add(warbandSlot);
        }

        public virtual void RemoveSquad(WarbandSlot warbandSlot)
        {
            warbandSlot.RemoveUnit();
        }

        public virtual bool AddSquad(UnitDataSO unitDataSO)
        {
            for (int i = 0; i < _warbandSlotList.Count; i++)
            {
                if (!_warbandSlotList[i].UnitEmpty) continue;

                _warbandSlotList[i].UpdateUnitData(unitDataSO);
                return true;
            }

            Debug.Log($"Not enough space in warband for {unitDataSO.Name}");
            return false;
        }

        protected virtual void ReplaceWarbandSlotSquad(WarbandSlot replaceSlot, UnitDataSO unitDataSO)
        {
            replaceSlot.UpdateUnitData(unitDataSO);
        }

        protected virtual void AddEquipmentSlotToFirstEmptySlot(EquipmentData equipmentData)
        {
            for (int i = 0; i < _warbandSlotList.Count; i++)
            {
                if (!_warbandSlotList[i].CanFitNewEqupment) continue;

                _warbandSlotList[i].AddEquipmentData(equipmentData);
                return;
            }

            Debug.Log($"There is no space in warband slots for item: {equipmentData.Name}");
        }

        private void InitializeWarband()
        {
            for (int i = 0; i < _warbandSize; i++)
                AddEmptyWarbandSlot();
        }
    }
}
