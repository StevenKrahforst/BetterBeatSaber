using BetterBeatSaber.Extensions;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using IPA.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BeatmapObjectsInstaller))]
internal static class BeatmapObjectsInstallerMixin {

    private static readonly int _SimpleColor = Shader.PropertyToID("_SimpleColor");
    
    private static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor FirstBombMaterialAccessor = FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material0");
    private static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor SecondBombMaterialAccessor = FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material1");

    private static readonly Color DefaultBombColor = Color.black.WithAlpha(0);

    private static Color? FirstBombColor, SecondBombColor;
    
    [MixinMethod(nameof(InstallBindings), MixinAt.Post)]
    private static void InstallBindings(
        // ReSharper disable once SuggestBaseTypeForParameter
        BombNoteController ____bombNotePrefab
    ) {

        var conditionalMaterialSwitcher = ____bombNotePrefab.GetComponentInChildren<ConditionalMaterialSwitcher>();
        if(conditionalMaterialSwitcher == null)
            return;
        
        var firstBombMaterial = FirstBombMaterialAccessor(ref conditionalMaterialSwitcher);
        var secondBombMaterial = FirstBombMaterialAccessor(ref conditionalMaterialSwitcher);
        
        FirstBombColor ??= firstBombMaterial.GetColor(_SimpleColor);
        SecondBombColor ??= secondBombMaterial.GetColor(_SimpleColor);
        
        switch (BetterBeatSaberConfig.Instance.Bombs.Colorize && BetterBeatSaberConfig.Instance.Bombs.RGB) {
            case true when !BetterBeatSaberConfig.Instance.Bombs.RGB:
                if(BetterBeatSaberConfig.Instance.Bombs.FirstColor != null)
                    firstBombMaterial.SetColor(_SimpleColor, BetterBeatSaberConfig.Instance.Bombs.FirstColor.HasValue ? BetterBeatSaberConfig.Instance.Bombs.FirstColor.Value : DefaultBombColor);
                if(BetterBeatSaberConfig.Instance.Bombs.SecondColor != null)
                    firstBombMaterial.SetColor(_SimpleColor, BetterBeatSaberConfig.Instance.Bombs.SecondColor.HasValue ? BetterBeatSaberConfig.Instance.Bombs.SecondColor.Value : DefaultBombColor);
                break;
            case false:
                firstBombMaterial.SetColor(_SimpleColor, FirstBombColor ?? DefaultBombColor);
                secondBombMaterial.SetColor(_SimpleColor, SecondBombColor ?? DefaultBombColor);
                break;
        }
        
    }

}