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
    // ReSharper disable once RedundantAssignment
    private static void Init(ref Color ____initColor) =>
        ____initColor = Manager.ColorManager.Instance.FirstColor;

    [MixinMethod(nameof(Update), MixinAt.Pre)]
    // ReSharper disable once RedundantAssignment
    private static void Update(ref Color ____initColor) =>
        ____initColor = Manager.ColorManager.Instance.FirstColor;
    
}