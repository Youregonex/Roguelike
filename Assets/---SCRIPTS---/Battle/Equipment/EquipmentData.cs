using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

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

        [JsonIgnore] private Sprite _icon;

        [JsonIgnore]
        public Sprite Icon
        {
            get
            {
                if (_icon == null) _icon = ResourceLoader.GetIconWithPath(IconPath);
                return _icon;
            }
        }

        public bool IsEmpty => string.IsNullOrEmpty(Name);
    }
}