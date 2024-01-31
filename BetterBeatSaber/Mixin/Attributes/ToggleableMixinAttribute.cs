using System;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ToggleableMixinAttribute(
    Type configType,
    string propertyName
) : Attribute {

    public Type ConfigType { get; } = configType;
    public string PropertyName { get; } = propertyName;

}