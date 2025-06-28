using UnityEngine;

namespace Yg.GameData.Configs
{
    [CreateAssetMenu(fileName = "ResourceConfig", menuName = "Configs/PointsOfInterest/ResourceConfigSO")]
    public class ResourcePointOfInterestConfigSO : BasePointOfInterestConfigSO
    {
        [field: SerializeField] public int ResourceAmount { get; private set; }

        private void OnValidate()
        {
            Validate();

            if (ResourceAmount < 0) ResourceAmount *= -1;
        }
    }
}
