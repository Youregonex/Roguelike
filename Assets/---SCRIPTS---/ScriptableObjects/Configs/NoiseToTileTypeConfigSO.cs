using UnityEngine;
using System.Collections.Generic;

namespace Yg.GameConfigs
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

                if(i == 0)
                {
                    NoiseToTileTypeList[i].ThresholdMin = 0;
                }

                if(i == NoiseToTileTypeList.Count - 1)
                {
                    NoiseToTileTypeList[i].ThresholdMax = 1;
                }

                if(i + 1 < NoiseToTileTypeList.Count)
                {
                    NoiseToTileTypeList[i + 1].ThresholdMin = NoiseToTileTypeList[i].ThresholdMax;
                }
            }
        }
    }

    [System.Serializable]
    public class NoiseToTileType
    {
        [field: SerializeField, Range(0f, 1f)] public float ThresholdMin { get; internal set; }
        [field: SerializeField, Range(0f, 1f)] public float ThresholdMax { get; internal set; }
        [field: SerializeField] public ETileType TileType { get; internal set; }

        public void Validate()
        {
            if (ThresholdMin >= ThresholdMax)
            {                
                ThresholdMax = ThresholdMin + 0.01f;
            }
        }
    }
}
