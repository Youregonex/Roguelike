using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using UnityEngine;

namespace Yg.MapGeneration
{
    [JsonConverter(typeof(PointOfInterestConverter))]
    public abstract class BasePointOfInterest : IPointOfInterest
    {
        [JsonProperty] public Vector2Int PointPosition { get; protected set; }
        [JsonProperty] public EPointOfInterestType PointType { get; protected set; }

        public BasePointOfInterest() { }

        public BasePointOfInterest(Vector2Int position)
        {
            PointPosition = position;
        }

        public abstract void Interact();
        public abstract Tile GetPointTile();
    }
}