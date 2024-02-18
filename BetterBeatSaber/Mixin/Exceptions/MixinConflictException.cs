using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.Exceptions;

public sealed class MixinConflictException(
    MixinObject obj,
    string with
) : MixinObjectException(MixinError.Conflict, obj, BuildMessage(obj, with)) {

    private static string BuildMessage(MixinObject obj, string with) =>
        obj switch {
            Mixin mixin => $"Mixin {mixin.Type.Name} conflicts with Plugin {with}",
            MixinMethod mixinMethod => $"Method {mixinMethod.PatchMethod.Name} of Mixin {mixinMethod.Mixin.Type.Name} conflicts with Plugin {with}",
            _ => string.Empty
        };
    
}