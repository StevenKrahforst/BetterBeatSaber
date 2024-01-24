using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BombExplosionEffect))]
internal static class BombExplosionEffectMixin {

    [MixinMethod(nameof(SpawnExplosion), MixinAt.Pre)]
    private static bool SpawnExplosion() => !BetterBeatSaberConfig.Instance.DisableBombExplosionEffect;

}