using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(NoteCutParticlesEffect))]
internal static class NoteCutParticlesEffectMixin {

    // TODO: Check if already being done by smth else
    [MixinMethod(nameof(SpawnParticles), MixinAt.Pre)]
    private static bool SpawnParticles(ref Color32 color) {
        
        if (BetterBeatSaberConfig.Instance.DisableCutParticles)
            return false;
        
        if (BetterBeatSaberConfig.Instance.ColorizeCutParticles)
            color = RGB.Instance.FirstColor;
        
        return true;
        
    }

    [MixinMethod(nameof(Awake), MixinAt.Pre)]
    [ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.DisableCutParticles))]
    private static bool Awake() => false;

}