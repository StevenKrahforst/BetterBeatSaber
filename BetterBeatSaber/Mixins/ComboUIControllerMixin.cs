using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(ComboUIController))]
internal static class ComboUIControllerMixin {

    [MixinMethod(nameof(Start), MixinAt.Post)]
    private static void Start(Animator ____animator) {
        if (BetterBeatSaberConfig.Instance.DisableComboBreakEffect)
            ____animator.speed = 69420f;
    }

}