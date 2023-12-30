using System;

using Newtonsoft.Json;

using UnityEngine;

namespace BetterBeatSaber.Config.Converters; 

public sealed class ColorConverter : JsonConverter<Color> {

    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer) {
        writer.WriteValue($"#{ColorUtility.ToHtmlStringRGB(value)}");
    }

    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer) {
        return ColorUtility.TryParseHtmlString((string) reader.Value!, out var color) ? color : existingValue;
    }

}