using System.Collections.Generic;
using UnityEngine;

namespace Yg.GameData.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentRarityTierStatsConfig", menuName = "Configs/Equipment/RarityTierStats")]
    public class EquipmentRarityTierStatsConfig : ScriptableObject
    {
        [field: SerializeField] public List<EquipmentRarityTierStats> EquipmentRarityTierStatsList { get; private set; }
    }

    [System.Serializable]
    public class EquipmentRarityTierStats
    {
        [field: SerializeField] public EEquipmentRarity EquipmentRarity { get; private set; }
        [field: SerializeField] public int Tier { get; private set; }
        [field: SerializeField] public int AmountOfStatsMin { get; private set; }
        [field: SerializeField] public int AmountOfStatsMax { get; private set; }
        [field: SerializeField] public float StatValueChangeMin { get; private set; }
        [field: SerializeField] public float StatValueChangeMax { get; private set; }
        [field: SerializeField] public int PerksAmountMin { get; private set; }
        [field: SerializeField] public int PerksAmountMax { get; private set; }
    }
}
