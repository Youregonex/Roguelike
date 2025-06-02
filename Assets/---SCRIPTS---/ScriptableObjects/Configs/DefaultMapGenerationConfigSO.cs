using UnityEngine;

namespace Yg.GameConfigs
{
    [CreateAssetMenu(fileName = "DefaultMapGenerationConfig", menuName = "Configs/Generation/DefaultMapGenerationConfigSO")]
    public class DefaultMapGenerationConfigSO : ScriptableObject
    {
        [field: SerializeField] public int MapWidth { get; private set; }
        [field: SerializeField] public int MapHeight { get; private set; }
    }
}

