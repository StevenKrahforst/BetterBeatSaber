using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BombExplosionEffect))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.DisableBombExplosionEffect))]
internal static class BombExplosionEffectMixin {

    [MixinMethod(nameof(SpawnExplosion), MixinAt.Pre)]
    private static bool SpawnExplosion() => false;

}