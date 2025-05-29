using UnityEngine;
using System.Collections.Generic;

namespace Y.Configs
{
    [CreateAssetMenu(fileName = "NoiseToTileTypeConfigSO", menuName = "Configs/Tiles/NoiseToTileTypeConfigSO")]
    public class NoiseToTileTypeConfigSO : ScriptableObject
    {
        [field: SerializeField] public List<NoiseToTileType> NoiseToTileTypeList { get; private set; }

        public ETileType GetTiletypeWithNoise(float noiseValue)
        {
            foreach (var noiseToTileType in NoiseToTileTypeList)
            {
                if (noiseValue >= noiseToTileType.ThresholdMin && noiseValue <= noiseToTileType.ThresholdMax)
                    return noiseToTileType.TileType;
            }

            Debug.Log($"Couldn't find TileType for {noiseValue} noise value!");
            return ETileType.None;
        }

        private void OnValidate()
        {
            for (int i = 0; i < NoiseToTileTypeList.Count; i++)
            {
                NoiseToTileTypeList[i].Validate();

                for (int j = i + 1; j < NoiseToTileTypeList.Count; j++)
                {
                    if (NoiseToTileTypeList[j].ThresholdMin < NoiseToTileTypeList[i].ThresholdMax)
                    {
                        Debug.LogWarning("Overlapping noise thresholds!");
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class NoiseToTileType
    {
        [field: SerializeField, Range(0f, 1f)] public float ThresholdMin { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float ThresholdMax { get; private set; }
        [field: SerializeField] public ETileType TileType { get; private set; }

        public void Validate()
        {
            if (ThresholdMin >= ThresholdMax)
            {
                Debug.LogWarning($"Invalid thresholds in {nameof(NoiseToTileType)}: Min ({ThresholdMin}) >= Max ({ThresholdMax}). Adjusting automatically.");
                ThresholdMax = ThresholdMin + 0.01f;
            }
        }
    }
}
