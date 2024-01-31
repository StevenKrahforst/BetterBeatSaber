using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(LightSwitchEventEffect))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.HideLevelEnvironment))]
internal static class LightSwitchEventEffectMixin {

    [MixinMethod(nameof(Start), MixinAt.Pre)]
    private static bool Start() => false;

}