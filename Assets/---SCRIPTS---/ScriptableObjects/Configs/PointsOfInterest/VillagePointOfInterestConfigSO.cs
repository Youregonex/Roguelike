using UnityEngine;

namespace Yg.GameData.Configs
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
