using UnityEngine;
using Yg.GameConfigs;

namespace Yg.MapGeneration
{
    public class CastlePoint : BasePointOfInterest
    {
        public int CastleAreaWidth { get; private set; }
        public int CastleAreaHeight { get; private set; }

        public Vector2Int CastleOrigin { get; private set; }

        public CastlePoint(CastlePointOfInterestConfigSO castlePointOfInterestConfigSO, Vector2Int origin) : base(castlePointOfInterestConfigSO)
        {
            CastleAreaWidth = castlePointOfInterestConfigSO.CastleAreaWidth;
            CastleAreaHeight = castlePointOfInterestConfigSO.CastleAreaHeight;

            CastleOrigin = origin;
        }

        public override void Interact()
        {
            Debug.Log("Castle");
        }
    }
}
