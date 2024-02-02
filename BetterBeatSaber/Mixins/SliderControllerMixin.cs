using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(SliderController))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.ColorizeArcs))]
internal static class SliderControllerMixin {

    [MixinMethod(nameof(Init), MixinAt.Post)]
    private static void Init(ref Color ____initColor) {
        if(Manager.ColorManager.Instance != null)
            ____initColor = Manager.ColorManager.Instance.FirstColor;
    }

    [MixinMethod(nameof(Update), MixinAt.Pre)]
    private static void Update(ref Color ____initColor) {
        if(Manager.ColorManager.Instance != null)
            ____initColor = Manager.ColorManager.Instance.FirstColor;
    }
    
}