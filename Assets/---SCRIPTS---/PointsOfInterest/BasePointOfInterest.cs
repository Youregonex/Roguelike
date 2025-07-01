using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using UnityEngine;
using Yg.Converters;
using Yg.Character;

namespace Yg.MapGeneration
{
    [JsonConverter(typeof(PointOfInterestConverter))]
    public abstract class BasePointOfInterest : IPointOfInterest
    {
        [JsonProperty] public Vector2Int PointPosition { get; protected set; }
        [JsonProperty] public EPointOfInterestType PointType { get; protected set; }
        [JsonProperty] public bool Visited { get; protected set; }

        public BasePointOfInterest() { }

        public BasePointOfInterest(Vector2Int position)
        {
            PointPosition = position;
        }

        public abstract void Interact(PlayerCore playerCore);
        public abstract Tile GetPointTile();
    }
}