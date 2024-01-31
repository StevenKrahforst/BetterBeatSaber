using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(StaticEnvironmentLights))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.HideLevelEnvironment))]
internal static class StaticEnvironmentLightsMixin {

    [MixinMethod(nameof(Awake), MixinAt.Pre)]
    private static bool Awake() => false;

}