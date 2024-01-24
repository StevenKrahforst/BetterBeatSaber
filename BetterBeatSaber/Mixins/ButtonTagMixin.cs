using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[MixinPlugin("BeatSaberMarkupLanguage", "BeatSaberMarkupLanguage.Tags.ButtonTag")]
internal static class ButtonTagMixin {

    [MixinMethod(nameof(CreateObject), MixinAt.Post)]
    private static void CreateObject(GameObject __result) {
        if (!BetterBeatSaberConfig.Instance.ColorizeButtons)
            return;
        __result.AddComponent<ImageViewColorizer>();
    }

}