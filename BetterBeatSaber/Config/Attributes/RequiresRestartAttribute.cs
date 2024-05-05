using System;

namespace BetterBeatSaber.Config.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RequiresRestartAttribute(RequiresRestartAttribute.RestartType type = RequiresRestartAttribute.RestartType.Simple) : Attribute {

    public RestartType Type { get; set; } = type;
    
    public enum RestartType {

        Simple,
        Full

    }

}