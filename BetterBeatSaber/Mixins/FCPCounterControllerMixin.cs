using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Providers;

using TMPro;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin("FCPercentage", "FCPercentage.FCPCounter.FCPCounterController")]
internal static class FCPCounterControllerMixin {

    [MixinMethod(nameof(InitCounterText), MixinAt.Post)]
    private static void InitCounterText(ref TMP_Text ___counterPercentageText) {
        if (BetterBloomFontProvider.Instance != null && BetterBeatSaberConfig.Instance.ColorizeFCPercentage)
            ___counterPercentageText.font = BetterBloomFontProvider.Instance.BloomFont;
    }

}