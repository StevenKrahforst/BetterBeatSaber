using BetterBeatSaber.Mixin;
using BetterBeatSaber.Providers;

using TMPro;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[MixinPlugin("PBOT", "PBOT.Managers.DeltaRankCounterVisualManager")]
internal static class DeltaRankCounterVisualManagerMixin {

    [MixinMethod(nameof(CounterInit), MixinAt.Post)]
    private static void CounterInit(ref TMP_Text ____text) {
        if(BloomFontProvider.Instance != null && BetterBeatSaberConfig.Instance.ColorizePBOT)
            ____text.font = BloomFontProvider.Instance.BloomFont;
    }

}