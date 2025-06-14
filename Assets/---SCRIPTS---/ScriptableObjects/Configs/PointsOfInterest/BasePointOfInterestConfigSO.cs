using UnityEngine;
using UnityEngine.Tilemaps;

namespace Yg.GameConfigs
{
    public abstract class BasePointOfInterestConfigSO : ScriptableObject
    {
        [field: SerializeField] public int PointsAmountMin { get; private set; }
        [field: SerializeField] public int PointsAmountMax { get; private set; }
        [field: SerializeField] public Tile PointTile { get; protected set; }

        private void OnValidate()
        {
            Validate();
        }

        protected void Validate()
        {
            if (PointsAmountMin < 0) PointsAmountMin *= -1;
            if (PointsAmountMax < 0) PointsAmountMax *= -1;
            if (PointsAmountMax < PointsAmountMin) PointsAmountMax = PointsAmountMin;
        }
    }
}
