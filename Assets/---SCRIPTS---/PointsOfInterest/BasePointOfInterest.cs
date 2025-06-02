using UnityEngine.Tilemaps;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public abstract class BasePointOfInterest : IPointOfInterest
    {
        public EPointOfInterestType PointType { get; private set; }
        public Tile PointTile { get; private set; }

        public BasePointOfInterest(BasePointOfInterestConfigSO basePointOfInterestConfigSO)
        {
            PointType = basePointOfInterestConfigSO.PointType;
            PointTile = basePointOfInterestConfigSO.PointTile;
        }

        public abstract void Interact();
    }
}
