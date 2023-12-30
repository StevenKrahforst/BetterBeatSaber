using System;

using Newtonsoft.Json;

using UnityEngine;

namespace BetterBeatSaber.Config.Converters; 

public sealed class QuaternionConverter : JsonConverter<Quaternion> {

    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer) {
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("z");
        writer.WriteValue(value.z);
        writer.WritePropertyName("w");
        writer.WriteValue(value.w);
        writer.WriteEndObject();
    }

    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer) {
        return JsonConvert.DeserializeObject<Quaternion>(serializer.Deserialize(reader)!.ToString());
    }

}