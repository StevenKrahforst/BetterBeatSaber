using System;

namespace BetterBeatSaber.Mixin;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MixinMethodAttribute : Attribute {

    public string MethodName { get; set; }
    public MixinAt At { get; set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public MixinMethodAttribute(string methodName, MixinAt at) {
        MethodName = methodName;
        At = at;
    }

}