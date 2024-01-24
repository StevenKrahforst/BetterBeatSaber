using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(LightSwitchEventEffect))]
internal static class LightSwitchEventEffectMixin {

    [MixinMethod(nameof(Start), MixinAt.Pre)]
    private static bool Start() => !BetterBeatSaberConfig.Instance.HideLevelEnvironment;

}