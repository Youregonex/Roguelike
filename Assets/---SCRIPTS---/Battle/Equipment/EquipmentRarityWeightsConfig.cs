using System.Collections.Generic;
using UnityEngine;

namespace Yg.GameData.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentRarityWeightConfig", menuName = "Configs/Equipment/RarityWeight")]
    public class EquipmentRarityWeightsConfig : ScriptableObject
    {
        [field: SerializeField] public List<RarityWeight> RarityWeightList { get; private set; }
    }

    [System.Serializable]
    public class RarityWeight
    {
        [field: SerializeField] public EEquipmentRarity Rarity { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }
    }
}
