using System;
using Newtonsoft.Json.Linq;
using Yg.Character;

namespace Yg.Converters
{
    public class CharacterSaveDataConverter : JsonCreationConverter<CharacterSaveData>
    {
        protected override CharacterSaveData Create(Type objectType, JObject jObject)
        {
            ECharacterSaveDataType type = (ECharacterSaveDataType)(int)jObject["CharacterSaveDataType"];

            return type switch
            {
                ECharacterSaveDataType.Default=> new CharacterSaveData(),
                ECharacterSaveDataType.Player => new PlayerSaveData(),
                _ => null,
            };
        }
    }
}
