using BetterBeatSaber.Mixin;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BeatmapData))]
internal static class BeatmapDataMixin {

    [MixinMethod(nameof(InsertBeatmapEventData), MixinAt.Pre)]
    private static bool InsertBeatmapEventData() => !BetterBeatSaberConfig.Instance.HideLevelEnvironment;

}