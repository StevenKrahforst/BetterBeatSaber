using System;

using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MixinMethodAttribute : Attribute {

    public string MethodName { get; set; }
    public MixinAt At { get; set; }
    public string[] ConflictsWith { get; set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public MixinMethodAttribute(string methodName, MixinAt at, params string[] conflictsWith) {
        MethodName = methodName;
        At = at;
        ConflictsWith = conflictsWith;
    }

}