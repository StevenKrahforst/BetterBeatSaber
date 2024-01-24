using System;

using BetterBeatSaber.Utilities;

using Newtonsoft.Json;

namespace BetterBeatSaber.Config.Converters;

public sealed class ObservableValueConverter<T> : JsonConverter<ObservableValue<T>> {

    public override void WriteJson(JsonWriter writer, ObservableValue<T>? value, JsonSerializer serializer) {
        if (value != null)
            writer.WriteValue(value.CurrentValue);
        else
            writer.WriteNull();
    }

    public override ObservableValue<T> ReadJson(JsonReader reader, Type objectType, ObservableValue<T>? existingValue, bool hasExistingValue, JsonSerializer serializer) {

        Console.WriteLine(reader.Path + " - " + reader.ValueType?.Name + " - " + typeof(T).Name);
        
        T value;
        if (reader.ValueType != typeof(T)) {
            if (reader.ValueType == typeof(double))
                value = (T) (object) Convert.ToSingle(reader.Value);
            else
                throw new JsonException($"Cannot convert {reader.ValueType} to {typeof(T)}");
        } else
            value = (T) reader.Value!;
        
        if(value == null)
            throw new JsonException($"Cannot convert {reader.ValueType} because it is null");
        
        if (existingValue == null || !hasExistingValue)
            return new ObservableValue<T>(value);
        
        existingValue.SetValue(value);
        
        return existingValue;

    }
    
}