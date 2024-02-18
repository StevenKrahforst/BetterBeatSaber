using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(NoteDebris))]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.ColorizeNoteDebris))]
internal static class NoteDebrisMixin {

    private static readonly int ColorShaderPropertyId = Shader.PropertyToID("_Color");
    
    // ig no colorisation when paused
    
    [MixinMethod(nameof(Init), MixinAt.Post)]
    [MixinMethod("Update", MixinAt.Post)]
    private static void Init(MaterialPropertyBlockController ____materialPropertyBlockController) {
        
        if (Manager.ColorManager.Instance == null)
            return;
        
        var materialPropertyBlock = ____materialPropertyBlockController.materialPropertyBlock;
        materialPropertyBlock.SetColor(ColorShaderPropertyId, Manager.ColorManager.Instance.FirstColor);
        ____materialPropertyBlockController.ApplyChanges();
        
    }

}