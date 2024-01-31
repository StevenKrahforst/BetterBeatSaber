using System;

using BetterBeatSaber.Mixin.Enums;

using HarmonyLib;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MixinPropertyAttribute(
    string propertyName,
    MixinAt at,
    MethodType methodType
) : Attribute {

    public string PropertyName { get; set; } = propertyName;
    public MixinAt At { get; set; } = at;
    public MethodType MethodType { get; set; } = methodType;

}