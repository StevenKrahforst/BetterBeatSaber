using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using UnityEngine.UI;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(StandardLevelDetailView))]
internal static class StandardLevelDetailViewMixin {

    [MixinMethod(nameof(Awake), MixinAt.Post)]
    private static void Awake(ref Button ____actionButton, ref Button ____practiceButton) {
        
        if (!BetterBeatSaberConfig.Instance.ColorizeButtons)
            return;
        
        ____actionButton.gameObject.AddComponent<ImageColorizer>();
        ____practiceButton.gameObject.AddComponent<ImageColorizer>();
        
    }
    
}