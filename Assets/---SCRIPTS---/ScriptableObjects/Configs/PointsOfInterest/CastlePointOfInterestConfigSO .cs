using UnityEngine;

namespace Yg.GameConfigs
{
    [CreateAssetMenu(fileName = "CastleConfig", menuName = "Configs/PointsOfInterest/CastleConfigSO")]
    public class CastlePointOfInterestConfigSO : BasePointOfInterestConfigSO
    {
        [field: SerializeField] public int CastleAreaWidth { get; private set; }
        [field: SerializeField] public int CastleAreaHeight { get; private set; }

        private void OnValidate()
        {
            Validate();

            if (CastleAreaWidth % 2 == 0) CastleAreaWidth++;
            if (CastleAreaWidth < 0) CastleAreaWidth *= -1;

            if (CastleAreaHeight % 2 == 0) CastleAreaHeight++;
            if (CastleAreaHeight < 0) CastleAreaHeight *= -1;
        }
    }
}