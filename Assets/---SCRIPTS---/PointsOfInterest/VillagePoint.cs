using UnityEngine;
using UnityEngine.Tilemaps;
using Yg.Character;
using Yg.GameData.Configs;

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

        public override void Interact(PlayerCore playerCore)
        {
            if (Visited) return;

            playerCore.GetCharacterComponent<PlayerWarbandComponent>().InitiateUnitSelction();
            Visited = true;
        }

        public override Tile GetPointTile()
        {
            return ResourceLoader.CONFIG_VillagePointOfInterest.PointTile;
        }
    }
}
