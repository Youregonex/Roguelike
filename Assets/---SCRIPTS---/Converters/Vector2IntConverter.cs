using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Vector2IntConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var vector = (Vector2Int)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(vector.x);
        writer.WritePropertyName("y");
        writer.WriteValue(vector.y);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        return new Vector2Int(
            (int)jsonObject["x"],
            (int)jsonObject["y"]
        );
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Vector2Int);
    }
}