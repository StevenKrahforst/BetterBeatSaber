using System;

namespace BetterBeatSaber.Reflection;

public sealed class ReflectedEnum {
    
    public Type Type { get; set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public ReflectedEnum(Type type) {
        Type = type;
    }

    public object GetValue(string memberName) =>
        Enum.Parse(Type, memberName);

    public static ReflectedEnum Create(Type type) => new(type);

}