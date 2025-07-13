using System.Collections.Generic;
using UnityEngine;
using Yg.GameData.Equipment;

namespace Yg.GameData.Configs
{
    [CreateAssetMenu(fileName = "RarityColorConfig", menuName = "Configs/RarityColorConfig")]
    public class RarityColorConfig : ScriptableObject
    {
        [field: SerializeField] public List<RarityToColor> RarirtyToColorList { get; private set; }

        public Color GetColor(EEquipmentRarity equipmentRarity)
        {
            for (int i = 0; i < RarirtyToColorList.Count; i++)
            {
                if (RarirtyToColorList[i].EquipmentRarity == equipmentRarity)
                    return RarirtyToColorList[i].RarityColor;
            }

            Debug.LogWarning($"Couldn't find color for {equipmentRarity} rarity!");
            return Color.white;
        }
    }

    [System.Serializable]
    public class RarityToColor
    {
        [field: SerializeField] public EEquipmentRarity EquipmentRarity { get; private set; }
        [field: SerializeField] public Color RarityColor { get; private set; }
    }
}
