using System.Collections.Generic;
using Yg.GameData.Equipment;
using Yg.GameData.Units;

namespace Yg.Character
{
    [System.Serializable]
    public class WarbandSlot
    {
        private const int MAX_EQUIPMENT_SLOTS = 4;

        private UnitDataSO _unitData;
        private int _defaultSlotSize;

        private List<EquipmentData> _equipmentDataList = new(MAX_EQUIPMENT_SLOTS);

        public List<EquipmentData> EquipmentDataList => _equipmentDataList;
        public UnitDataSO UnitData => _unitData;
        public int DefaultSlotSize => _defaultSlotSize;

        public bool UnitEmpty => _unitData == null;
        public bool CanFitNewEqupment
        {
            get
            {
                for (int i = 0; i < MAX_EQUIPMENT_SLOTS; i++)
                {
                    if (_equipmentDataList[i] == null) return true;
                }

                return false;
            }
        }

        public int SlotSize
        {
            get
            {
                if (_unitData == null) return 0;

                return _unitData.DefaultSquadSize + _defaultSlotSize;
            }
        }

        public WarbandSlot()
        {
            for (int i = 0; i < MAX_EQUIPMENT_SLOTS; i++)
                _equipmentDataList.Add(null);
        }

        public WarbandSlot(UnitDataSO unitDataSO)
        {
            _unitData = unitDataSO;
        }

        public void AddEquipmentDataToFirstEmptySlot(EquipmentData equipmentData)
        {
            for (int i = 0; i < MAX_EQUIPMENT_SLOTS; i++)
                if (_equipmentDataList[i] == null)
                {
                    _equipmentDataList[i] = equipmentData;
                    return;
                }
        }

        public void AddEquipmentDataToSlot(int index, EquipmentData equipmentData)
        {
            _equipmentDataList[index] = equipmentData;
        }

        public void RemoveEquipmentDataFromSlot(int index)
        {
            _equipmentDataList[index] = null;
        }

        public void UpdateUnitData(UnitDataSO unitDataSO)
        {
            _unitData = unitDataSO;
        }

        public void RemoveUnit()
        {
            _unitData = null;
        }
    }

    public class WarbandSlotSaveData
    {
        public string PrefabId;
        public int DefaultSlotSize;
        public List<EquipmentData> EquipmentDataList;

        public bool Empty => string.IsNullOrEmpty(PrefabId);

        public WarbandSlotSaveData() { }

        public WarbandSlotSaveData(WarbandSlot warbandSlot)
        {
            PrefabId = warbandSlot.UnitEmpty ? string.Empty : warbandSlot.UnitData.PrefabId;
            DefaultSlotSize = warbandSlot.DefaultSlotSize;
            EquipmentDataList = warbandSlot.EquipmentDataList;
        }
    }
}
