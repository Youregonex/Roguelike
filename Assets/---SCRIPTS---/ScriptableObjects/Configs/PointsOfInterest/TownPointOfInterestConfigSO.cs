using UnityEngine;

namespace Yg.GameConfigs
{
    [CreateAssetMenu(fileName = "TownConfig", menuName = "Configs/PointsOfInterest/TownPointOfInterestConfigSO")]
    public class TownPointOfInterestConfigSO : BasePointOfInterestConfigSO
    {
        private void OnValidate()
        {
            Validate();
            PointType = EPointOfInterestType.Town;
        }
    }
}
