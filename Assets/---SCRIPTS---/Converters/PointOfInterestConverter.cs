using System;
using Newtonsoft.Json.Linq;
using Yg.MapGeneration;

namespace Yg.Converters
{
    public class PointOfInterestConverter : JsonCreationConverter<BasePointOfInterest>
    {
        protected override BasePointOfInterest Create(Type objectType, JObject obj)
        {
            EPointOfInterestType type = (EPointOfInterestType)(int)obj["PointType"];

            return type switch
            {
                EPointOfInterestType.Resource => new ResourcePoint(),
                EPointOfInterestType.Castle => new CastlePoint(),
                EPointOfInterestType.Town => new TownPoint(),
                EPointOfInterestType.Village => new VillagePoint(),
                _ => null,
            };
        }
    }
}