using UnityEngine;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class ResourcePoint : BasePointOfInterest
    {
        public int ResourceAmount { get; private set; }

        public ResourcePoint(ResourcePointOfInterestConfigSO resourcePointOfInterestConfigSO) : base(resourcePointOfInterestConfigSO)
        {
            ResourceAmount = resourcePointOfInterestConfigSO.ResourceAmount;
        }

        public override void Interact()
        {
            Debug.Log($"Resource {ResourceAmount}");
        }
    }
}
