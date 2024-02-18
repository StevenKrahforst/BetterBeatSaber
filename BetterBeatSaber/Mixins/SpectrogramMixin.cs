using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(Spectrogram))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.HideLevelEnvironment))]
internal static class SpectrogramMixin {

    [MixinMethod(nameof(Update), MixinAt.Pre)]
    private static bool Update() => false;

}