using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BlueNoiseDitheringUpdater))]
internal static class BlueNoiseDitheringUpdaterMixin {

    private static readonly int GlobalNoiseTextureID = Shader.PropertyToID("_GlobalBlueNoiseTex");
    private static bool _lastDisableState;

    [MixinMethod(nameof(HandleCameraPreRender), MixinAt.Pre)]
    private static bool HandleCameraPreRender() {
        
        if(!BetterBeatSaberConfig.Instance.DisableMenuCameraNoise) {
            _lastDisableState = false;
            return true;
        }

        if (_lastDisableState)
            return false;
        
        Shader.SetGlobalTexture(GlobalNoiseTextureID, null);
        _lastDisableState = true;

        return false;
        
    }

}