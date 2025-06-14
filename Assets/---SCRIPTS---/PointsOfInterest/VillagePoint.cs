using UnityEngine;
using UnityEngine.Tilemaps;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class VillagePoint : BasePointOfInterest
    {

        public VillagePoint() { }

        public VillagePoint(
            VillagePointOfInterestConfigSO villagePointOfInterestConfigSO,
            Vector2Int position)
            : base(position)
        {
            PointType = EPointOfInterestType.Village;
        }

        public override void Interact()
        {
            Debug.Log("Village");
        }

        public override Tile GetPointTile()
        {
            return ResourceLoader.CONFIG_VillagePointOfInterest.PointTile;
        }
    }
}
