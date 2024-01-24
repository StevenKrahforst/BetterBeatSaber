using System;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ToggleableMixinAttribute : Attribute {

    public Type ConfigType { get; }
    public string PropertyName { get; }

    public ToggleableMixinAttribute(Type configType, string propertyName) {
        ConfigType = configType;
        PropertyName = propertyName;
    }

}