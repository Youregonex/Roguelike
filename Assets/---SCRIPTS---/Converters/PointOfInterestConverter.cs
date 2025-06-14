using System;
using Newtonsoft.Json.Linq;
using Yg.MapGeneration;

public class PointOfInterestConverter : JsonCreationConverter<BasePointOfInterest>
{
    protected override BasePointOfInterest Create(Type objectType, JObject obj)
    {
        EPointOfInterestType type = ((EPointOfInterestType)(int)obj["PointType"]);

        switch (type)
        {
            case EPointOfInterestType.Resource:
                return new ResourcePoint();
            case EPointOfInterestType.Castle:
                return new CastlePoint();
            case EPointOfInterestType.Town:
                return new TownPoint();
            case EPointOfInterestType.Village:
                return new VillagePoint();
            default:
                return null;
        }
    }
}