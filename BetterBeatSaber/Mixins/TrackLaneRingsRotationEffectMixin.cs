using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(TrackLaneRingsRotationEffect))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.HideLevelEnvironment))]
internal static class TrackLaneRingsRotationEffectMixin {

    [MixinMethod(nameof(Awake), MixinAt.Pre)]
    [MixinMethod("Start", MixinAt.Pre)]
    [MixinMethod("FixedUpdate", MixinAt.Pre)]
    [MixinMethod("AddRingRotationEffect", MixinAt.Pre)]
    [MixinMethod("GetFirstRingRotationAngle", MixinAt.Pre)]
    [MixinMethod("GetFirstRingDestinationRotationAngle", MixinAt.Pre)]
    [MixinMethod("SpawnRingRotationEffect", MixinAt.Pre)]
    [MixinMethod("RecycleRingRotationEffect", MixinAt.Pre)]
    private static bool Awake() => false;

}