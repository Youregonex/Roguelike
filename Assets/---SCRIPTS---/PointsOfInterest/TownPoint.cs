using UnityEngine;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class TownPoint : BasePointOfInterest
    {
        public TownPoint(TownPointOfInterestConfigSO townPointOfInterestConfigSO) : base(townPointOfInterestConfigSO) {}

        public override void Interact()
        {
            Debug.Log("Town");
        }
    }
}
