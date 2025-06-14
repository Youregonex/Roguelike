using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class ResourcePoint : BasePointOfInterest
    {
        [JsonProperty] public int ResourceAmount { get; private set; }

        public ResourcePoint() { }

        public ResourcePoint(
            ResourcePointOfInterestConfigSO resourcePointOfInterestConfigSO,
            Vector2Int position)
            : base(position)
        {
            PointType = EPointOfInterestType.Resource;
            ResourceAmount = resourcePointOfInterestConfigSO.ResourceAmount;
        }

        public override void Interact()
        {
            Debug.Log($"Resource {ResourceAmount}");
        }

        public override Tile GetPointTile()
        {
            return ResourceLoader.CONFIG_ResourcePointOfInterest.PointTile;
        }
    }
}
