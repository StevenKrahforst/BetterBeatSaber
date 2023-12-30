using BetterBeatSaber.Mixin;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(NoteCutParticlesEffect))]
internal static class NoteCutParticlesEffectMixin {

    [MixinMethod(nameof(SpawnParticles), MixinAt.Pre)]
    private static bool SpawnParticles(ref Color32 color) {
        
        if (BetterBeatSaberConfig.Instance.DisableCutParticles)
            return false;
        
        if (BetterBeatSaberConfig.Instance.ColorizeCutParticles)
            color = RGB.Instance.FirstColor;
        
        return true;
        
    }

    [MixinMethod(nameof(Awake), MixinAt.Pre)]
    private static bool Awake() =>
        BetterBeatSaberConfig.Instance.DisableCutParticles;

}