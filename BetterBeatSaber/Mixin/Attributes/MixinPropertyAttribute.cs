using System;

using HarmonyLib;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MixinPropertyAttribute : Attribute {

    public string PropertyName { get; set; }
    public MixinAt At { get; set; }
    public MethodType MethodType { get; set; }

    public MixinPropertyAttribute(string propertyName, MixinAt at, MethodType methodType) {
        PropertyName = propertyName;
        At = at;
        MethodType = methodType;
    }

}