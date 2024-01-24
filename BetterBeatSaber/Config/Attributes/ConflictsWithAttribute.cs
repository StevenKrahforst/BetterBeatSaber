using System;

namespace BetterBeatSaber.Config.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ConflictsWithAttribute : Attribute {

    public string ConflictsWith { get; }

    public ConflictsWithAttribute(string conflictsWith) {
        ConflictsWith = conflictsWith;
    }

}