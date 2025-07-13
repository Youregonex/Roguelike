using UnityEngine;
using Yg.GameData.Configs;
using Yg.GameData.Equipment;

namespace Yg.Systems
{
    public class ColorPicker
    {
        private const string RARITY_COLOR_CONFIG = "Configs/Colors/RarityColorConfig";

        private readonly RarityColorConfig _rarityColorConfig;

        public ColorPicker()
        {
            _rarityColorConfig = Resources.Load<RarityColorConfig>(RARITY_COLOR_CONFIG);
        }

        public Color GetColor(EEquipmentRarity equipmentRarity)
        {
            return _rarityColorConfig.GetColor(equipmentRarity);
        }
    }
}
