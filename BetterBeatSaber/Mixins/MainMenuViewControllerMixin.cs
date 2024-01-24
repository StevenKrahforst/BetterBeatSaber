using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using UnityEngine.UI;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(MainMenuViewController))]
internal static class MainMenuViewControllerMixin {

    [MixinMethod(nameof(DidActivate), MixinAt.Post)]
    private static void DidActivate(
        ref Button ____soloButton,
        ref Button ____partyButton,
        ref Button ____campaignButton,
        ref Button ____quitButton,
        ref Button ____howToPlayButton,
        ref Button ____beatmapEditorButton,
        ref Button ____multiplayerButton,
        ref Button ____optionsButton,
        ref Button ____musicPackPromoBanner
    ) {

        if (BetterBeatSaberConfig.Instance.ColorizeMenuButtons) {
            
            ____soloButton.gameObject.AddComponent<ImageViewColorizer>();
            ____partyButton.gameObject.AddComponent<ImageViewColorizer>();
            ____campaignButton.gameObject.AddComponent<ImageViewColorizer>();
            ____quitButton.gameObject.AddComponent<ImageViewColorizer>();
            ____howToPlayButton.gameObject.AddComponent<ImageViewColorizer>();
            ____multiplayerButton.gameObject.AddComponent<ImageViewColorizer>();
            ____optionsButton.gameObject.AddComponent<ImageViewColorizer>();

            if(!BetterBeatSaberConfig.Instance.HideEditorButton)
                ____beatmapEditorButton.gameObject.AddComponent<ImageViewColorizer>();

            
        }
        
        ____beatmapEditorButton.gameObject.SetActive(!BetterBeatSaberConfig.Instance.HideEditorButton);
        ____musicPackPromoBanner.gameObject.SetActive(!BetterBeatSaberConfig.Instance.HidePromotionButton);
        
    }

}