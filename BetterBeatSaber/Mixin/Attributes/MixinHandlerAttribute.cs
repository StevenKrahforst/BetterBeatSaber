using System;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MixinHandlerAttribute : Attribute {

    public MixinAction Action { get; }

    public MixinHandlerAttribute(MixinAction action) {
        Action = action;
    }

}