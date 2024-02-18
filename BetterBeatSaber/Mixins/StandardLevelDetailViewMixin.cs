using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using UnityEngine.UI;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(StandardLevelDetailView))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.ColorizeButtons))]
internal static class StandardLevelDetailViewMixin {

    [MixinMethod(nameof(Awake), MixinAt.Post)]
    private static void Awake(ref Button ____actionButton, ref Button ____practiceButton) {
        ____actionButton.gameObject.AddComponent<ImageColorizer>();
        ____practiceButton.gameObject.AddComponent<ImageColorizer>();
    }
    
}