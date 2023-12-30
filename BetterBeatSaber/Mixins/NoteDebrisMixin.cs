using BetterBeatSaber.Mixin;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(NoteDebris))]
internal static class NoteDebrisMixin {

    private static readonly int ColorShaderPropertyId = Shader.PropertyToID("_Color");
    
    [MixinMethod(nameof(Init), MixinAt.Post)]
    [MixinMethod("Update", MixinAt.Post)]
    private static void Init(MaterialPropertyBlockController ____materialPropertyBlockController) {
        
        if (!BetterBeatSaberConfig.Instance.ColorizeNoteDebris)
            return;
        
        var materialPropertyBlock = ____materialPropertyBlockController.materialPropertyBlock;
        materialPropertyBlock.SetColor(ColorShaderPropertyId, RGB.Instance.FirstColor);
        ____materialPropertyBlockController.ApplyChanges();
        
    }

}