using System;
using System.Collections.Generic;

using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MixinMethodAttribute : Attribute {

    public string MethodName { get; set; }
    public MixinAt At { get; set; }
    public IEnumerable<string> ConflictsWith { get; set; }
    public int Priority { get; set; } = MixinManager.DefaultPriority;
    public IEnumerable<string>? Before { get; set; }
    public IEnumerable<string>? After { get; set; }

    public MixinMethodAttribute(string methodName, MixinAt at, params string[] conflictsWith) {
        MethodName = methodName;
        At = at;
        ConflictsWith = conflictsWith;
    }

    public MixinMethodAttribute(string methodName, MixinAt at, IEnumerable<string> conflictsWith, int priority, IEnumerable<string>? before, IEnumerable<string>? after) {
        MethodName = methodName;
        At = at;
        ConflictsWith = conflictsWith;
        Priority = priority;
        Before = before;
        After = after;
    }

}