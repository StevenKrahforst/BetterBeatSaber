using System;

using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.Exceptions;

public sealed class MixinOriginalMethodNotFoundException(
    MixinError error,
    MixinMethod mixinMethod,
    Exception innerException
) : MixinObjectException(error, mixinMethod, BuildMessage(error, mixinMethod), innerException) {

    private static string BuildMessage(MixinError error, MixinMethod mixinMethod) =>
        error switch {
            MixinError.OriginalMethodAmbiguousMatch => $"Mixin {mixinMethod.Mixin.Type.Name} has ambiguous match for original method {mixinMethod.OriginalMethodName} and while trying to take an alternative route, an error occurred",
            MixinError.OriginalMethodNotFound => $"Mixin {mixinMethod.Mixin.Type.Name} is failing to found original method {mixinMethod.OriginalMethodName}",
            _ => string.Empty
        };
    
}