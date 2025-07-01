using Yg.GameData.Units;

namespace Yg.Character
{
    [System.Serializable]
    public class WarbandSlot
    {
        private UnitDataSO _unitData;
        private int _slotSize;

        public UnitDataSO UnitData => _unitData;
        public int SlotSize => _slotSize;
        public bool Empty => _unitData is null;

        public WarbandSlot() {}

        public WarbandSlot(UnitDataSO unitDataSO, int slotSize)
        {
            _unitData = unitDataSO;
            _slotSize = slotSize;
        }

        public void UpdateData(UnitDataSO unitDataSO, int amount)
        {
            _unitData = unitDataSO;
            _slotSize = amount;
        }

        public void ClearSlot()
        {
            _unitData = null;
            _slotSize = 0;
        }
    }

    public class WarbandSlotSaveData
    {
        public string PrefabId;
        public int SlotSize;
        public bool Empty => string.IsNullOrEmpty(PrefabId);

        public WarbandSlotSaveData() { }

        public WarbandSlotSaveData(WarbandSlot warbandSlot)
        {
            PrefabId = warbandSlot.Empty ? string.Empty : warbandSlot.UnitData.PrefabId;
            SlotSize = warbandSlot.Empty ? 0 : warbandSlot.SlotSize;
        }
    }
}
