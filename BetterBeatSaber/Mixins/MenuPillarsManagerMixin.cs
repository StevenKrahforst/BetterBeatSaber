using BetterBeatSaber.Extensions;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

// ReSharper disable RedundantAssignment
// ReSharper disable UnusedParameter.Local

[Mixin("MenuPillars", "MenuPillars.Managers.MenuPillarsManager")]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.ColorizeMenuPillars))]
internal static class MenuPillarsManagerMixin {

    [MixinMethod(nameof(LateTick), MixinAt.Pre)]
    private static void LateTick(object __instance) {
        var property = __instance.GetType().GetProperty("CurrentColor");
        var alpha = ((Color?) property?.GetValue(__instance))?.a ?? 1f;
        property?.SetValue(__instance, Manager.ColorManager.Instance.FirstColor.WithAlpha(alpha));
    }

}