﻿using BetterBeatSaber.Interops;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(NoteCutParticlesEffect))]
internal static class NoteCutParticlesEffectMixin {

    [MixinMethod(nameof(SpawnParticles), MixinAt.Pre)]
    private static void SpawnParticles(ref Color32 color) {
        if (BetterBeatSaberConfig.Instance.ColorizeCutParticles && Tweaks55.Instance != null && !Tweaks55.Instance.DisableCutParticles)
            color = Manager.ColorManager.Instance.FirstColor;
    }

}