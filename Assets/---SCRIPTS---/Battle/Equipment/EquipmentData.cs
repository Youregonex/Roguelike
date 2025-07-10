using System.Collections.Generic;

namespace Yg.GameData.Equipment
{
    [System.Serializable]
    public class EquipmentData
    {
        public string Name;
        public EEquipmentRarity Rarity;
        public int Tier;
        public string IconPath;

        public List<StatModifier> StatModifierList;
        public List<string> PerkIdList;

        public bool IsEmpty => string.IsNullOrEmpty(Name);
    }
}