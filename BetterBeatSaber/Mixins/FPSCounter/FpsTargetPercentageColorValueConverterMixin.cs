using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using UnityEngine;

namespace BetterBeatSaber.Mixins.FPSCounter;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin("FPSCounter", "FPS_Counter.Converters.FpsTargetPercentageColorValueConverter")]
internal static class FpsTargetPercentageColorValueConverterMixin {

    private const float RGBThreshold = .975f;
    
    internal static bool RGB;

    private static readonly Color Green = Color.green;
    private static readonly Color Yellow = Color.yellow;
    private static readonly Color Orange = new(1f, .64f, 0f);
    private static readonly Color Red = Color.red;

    [MixinMethod("Convert", MixinAt.Pre)]
    // ReSharper disable once RedundantAssignment
    private static bool Convert(float fpsTargetPercentage, ref Color __result) {

        RGB = fpsTargetPercentage > RGBThreshold;
        
        __result = fpsTargetPercentage switch {
            > RGBThreshold => Manager.ColorManager.Instance.FirstColor,
            > .95f => Green,
            > .7f => Yellow,
            > .5f => Orange,
            _ => Red
        };

        return false;
        
    }

}