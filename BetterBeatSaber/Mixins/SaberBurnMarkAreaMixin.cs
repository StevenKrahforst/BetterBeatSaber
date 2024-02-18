using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using IPA.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(SaberBurnMarkArea))]
internal static class SaberBurnMarkAreaMixin {

    [MixinMethod(nameof(Start), MixinAt.Post)]
    private static void Start(SaberBurnMarkArea __instance) {

        BurnMarkAreaColorizer? burnMarksColorizer;
        if (!BetterBeatSaberConfig.Instance.ColorizeBurnMarks) {
            burnMarksColorizer = __instance.GetComponent<BurnMarkAreaColorizer>();
            if (burnMarksColorizer != null)
                Object.Destroy(burnMarksColorizer);
            return;
        }
        
        __instance.enabled = true;
        
        if(!__instance.gameObject.activeInHierarchy)
            __instance.gameObject.SetActive(true);
        
        __instance.SetField("_burnMarksFadeOutStrength", .5f);

        if (__instance.GetComponent<BurnMarkAreaColorizer>() != null)
            return;
        
        burnMarksColorizer = __instance.gameObject.AddComponent<BurnMarkAreaColorizer>();
        
        burnMarksColorizer.saberBurnMarkArea = __instance;
        
    }

}