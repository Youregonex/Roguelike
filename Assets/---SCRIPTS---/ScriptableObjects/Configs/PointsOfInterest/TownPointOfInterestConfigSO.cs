using UnityEngine;

namespace Yg.GameData.Configs
{
    [CreateAssetMenu(fileName = "TownConfig", menuName = "Configs/PointsOfInterest/TownPointOfInterestConfigSO")]
    public class TownPointOfInterestConfigSO : BasePointOfInterestConfigSO
    {
        private void OnValidate()
        {
            Validate();
        }
    }
}
