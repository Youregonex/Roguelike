using UnityEngine;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class VillagePoint : BasePointOfInterest
    {
        public VillagePoint(VillagePointOfInterestConfigSO villagePointOfInterestConfigSO) : base(villagePointOfInterestConfigSO) {}

        public override void Interact()
        {
            Debug.Log("Village");
        }
    }
}
