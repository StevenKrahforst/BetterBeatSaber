using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Mixin;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BombNoteController))]
internal static class BombNoteControllerMixin {

    [MixinMethod(nameof(Init), MixinAt.Post)]
    private static void Init(ref BombNoteController __instance) {
        var bombColorizer = __instance.GetComponent<BombColorizer>();
        switch (BetterBeatSaberConfig.Instance.Bombs.Colorize && BetterBeatSaberConfig.Instance.Bombs.RGB) {
            case true when bombColorizer == null:
                __instance.gameObject.AddComponent<BombColorizer>();
                break;
            case false when bombColorizer != null:
                Object.Destroy(bombColorizer);
                break;
        }
        
    }

}