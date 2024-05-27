using System;

using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin("BeatSaberMarkupLanguage", "BeatSaberMarkupLanguage.Tags.ButtonTag")]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.ColorizeButtons))]
internal static class ButtonTagMixin {

    [MixinMethod(nameof(CreateObject), MixinAt.Post)]
    private static void CreateObject(GameObject __result) =>
        __result.AddComponent<ImageViewColorizer>();

}