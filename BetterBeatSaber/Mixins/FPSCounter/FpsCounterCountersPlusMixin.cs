using BetterBeatSaber.Extensions;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Providers;

using HMUI;

using TMPro;

namespace BetterBeatSaber.Mixins.FPSCounter;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin("FPSCounter", "FPS_Counter.Counters.FpsCounterCountersPlus")]
internal static class FpsCounterCountersPlusMixin {

    [MixinMethod("CounterInit", MixinAt.Post)]
    private static void CounterInit(ref TMP_Text ____counterText, ref ImageView ____ringImage) {
        
        if(!BetterBeatSaberConfig.Instance.ColorizeFPSCounter || MaterialProvider.Instance == null)
            return;
        
        ____counterText.font = TextMeshProExtensions.BloomFont;
        ____counterText.fontSharedMaterial = MaterialProvider.Instance.DistanceFieldMaterial;
        ____ringImage.material = MaterialProvider.Instance.DefaultUIMaterial;
        
    }
    
    [MixinMethod("Tick", MixinAt.Post)]
    private static void Tick(ref TMP_Text ____counterText, ref ImageView ____ringImage) {
        
        if (!FpsTargetPercentageColorValueConverterMixin.RGB)
            return;
        
        ____counterText.color = Manager.ColorManager.Instance.FirstColor;
        ____ringImage.color = Manager.ColorManager.Instance.FirstColor;
        
    }

}