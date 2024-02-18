using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(ObstacleController))]
internal static class ObstacleControllerMixin {

    [MixinMethod(nameof(Init), MixinAt.Post)]
    // ReSharper disable once SuggestBaseTypeForParameter
    private static void Init(ObstacleController __instance) {
        if(BetterBeatSaberConfig.Instance.ColorizeObstacles)
            __instance.gameObject.AddComponent<ObstacleColorizer>();
    }

}