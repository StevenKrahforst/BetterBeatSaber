using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(StaticEnvironmentLights))]
internal static class StaticEnvironmentLightsMixin {

    [MixinMethod(nameof(Awake), MixinAt.Pre)]
    private static bool Awake() => !BetterBeatSaberConfig.Instance.HideLevelEnvironment;

}