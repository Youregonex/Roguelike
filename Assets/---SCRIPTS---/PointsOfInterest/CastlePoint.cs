using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class CastlePoint : BasePointOfInterest
    {
        [JsonProperty] public Vector2Int CastleOrigin { get; private set; }

        public CastlePoint() { }

        public CastlePoint(
            CastlePointOfInterestConfigSO castlePointOfInterestConfigSO,
            Vector2Int origin,
            Vector2Int position)
            : base(position)
        {
            PointType = EPointOfInterestType.Castle;
            CastleOrigin = origin;
        }

        public override void Interact()
        {
            Debug.Log($"Castle origin: {CastleOrigin}");
        }

        public override Tile GetPointTile()
        {
            return ResourceLoader.CONFIG_CastlePointOfInterest.PointTile;
        }
    }
}
