using System;

using BetterBeatSaber.Config.Enums;

namespace BetterBeatSaber.Config.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RequiresRestartAttribute : Attribute {

    public RestartType Type { get; }

    // ReSharper disable once ConvertToPrimaryConstructor
    public RequiresRestartAttribute(RestartType type) {
        Type = type;
    }

}