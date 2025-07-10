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

        public bool UnitEmpty => _unitData is null;
        public bool CanFitNewEqupment => _equipmentDataList.Count < MAX_EQUIPMENT_SLOTS;

        public int SlotSize
        {
            get
            {
                if (_unitData == null) return 0;

                return _unitData.DefaultSquadSize + _defaultSlotSize;
            }
        }

        public WarbandSlot() {}

        public WarbandSlot(UnitDataSO unitDataSO)
        {
            _unitData = unitDataSO;
        }

        public void AddEquipmentData(EquipmentData equipmentData)
        {
            _equipmentDataList.Add(equipmentData);
        }

        public void RemoveEquipmentData(EquipmentData equipmentData)
        {
            _equipmentDataList.Remove(equipmentData);
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
