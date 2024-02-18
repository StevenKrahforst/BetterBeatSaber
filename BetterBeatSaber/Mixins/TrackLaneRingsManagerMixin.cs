using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(TrackLaneRingsManager))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.HideLevelEnvironment))]
internal static class TrackLaneRingsManagerMixin {

    [MixinMethod(nameof(Start), MixinAt.Pre)]
    [MixinMethod("FixedUpdate", MixinAt.Pre)]
    [MixinMethod("LateUpdate", MixinAt.Pre)]
    private static bool Start() => false;

}