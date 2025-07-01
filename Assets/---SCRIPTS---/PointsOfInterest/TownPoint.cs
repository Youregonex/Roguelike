using UnityEngine;
using UnityEngine.Tilemaps;
using Yg.Character;
using Yg.GameData.Configs;

namespace Yg.MapGeneration
{
    public class TownPoint : BasePointOfInterest
    {
        public TownPoint() { }

        public TownPoint(
            TownPointOfInterestConfigSO townPointOfInterestConfigSO,
            Vector2Int position)
            : base(position)
        {
            PointType = EPointOfInterestType.Town;
        }


        public override void Interact(PlayerCore playerCore)
        {
            playerCore.EncounterBattle();
        }

        public override Tile GetPointTile()
        {
            return ResourceLoader.CONFIG_TownPointOfInterest.PointTile;
        }
    }
}
