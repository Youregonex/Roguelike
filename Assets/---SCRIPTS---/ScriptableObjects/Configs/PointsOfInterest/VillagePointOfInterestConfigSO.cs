using UnityEngine;

namespace Yg.GameConfigs
{
    [CreateAssetMenu(fileName = "VillageConfig", menuName = "Configs/PointsOfInterest/VillageConfigSO")]
    public class VillagePointOfInterestConfigSO : BasePointOfInterestConfigSO
    {
        private void OnValidate()
        {
            Validate();
        }
    }
}
