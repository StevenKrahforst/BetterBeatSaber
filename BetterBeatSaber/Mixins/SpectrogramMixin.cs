using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(Spectrogram))]
internal static class SpectrogramMixin {

    [MixinMethod(nameof(Update), MixinAt.Pre)]
    private static bool Update() => !BetterBeatSaberConfig.Instance.HideLevelEnvironment;

}