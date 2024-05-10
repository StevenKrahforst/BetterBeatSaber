using BetterBeatSaber.Extensions;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using TMPro;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin("PBOT", "PBOT.Managers.DeltaRankCounterVisualManager")]
internal static class DeltaRankCounterVisualManagerMixin {

    [MixinMethod(nameof(CounterInit), MixinAt.Post)]
    private static void CounterInit(ref TMP_Text ____text) {
        if(BetterBeatSaberConfig.Instance.ColorizePBOT)
            ____text.font = TextMeshProExtensions.BloomFont;
    }

}