using BetterBeatSaber.Extensions;
using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using IPA.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// https://github.com/kinsi55/BeatSaber_Tweaks55/blob/master/HarmonyPatches/WallStuff.cs

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BeatmapObjectsInstaller))]
internal static class BeatmapObjectsInstallerMixin {

    private static readonly int _SimpleColor = Shader.PropertyToID("_SimpleColor");
    
    private static readonly FieldAccessor<ObstacleController, GameObject[]>.Accessor VisualWrappersFieldAccessor = FieldAccessor<ObstacleController, GameObject[]>.GetAccessor("_visualWrappers");
    
    private static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor FirstBombMaterialAccessor = FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material0");
    private static readonly FieldAccessor<ConditionalMaterialSwitcher, Material>.Accessor SecondBombMaterialAccessor = FieldAccessor<ConditionalMaterialSwitcher, Material>.GetAccessor("_material1");

    private static GameObject[]? _visualWrappersOriginal;
    
    private static readonly Color DefaultBombColor = Color.black.WithAlpha(0);

    private static Color? FirstBombColor, SecondBombColor;
    
    [MixinMethod(nameof(InstallBindings), MixinAt.Post)]
    private static void InstallBindings(
        ObstacleController ____obstaclePrefab,
        // ReSharper disable once SuggestBaseTypeForParameter
        BombNoteController ____bombNotePrefab
    ) {

        #region Transparent Obstacles

        if(_visualWrappersOriginal != null) {
            
            if(BetterBeatSaberConfig.Instance.TransparentObstacles)
                return;

            VisualWrappersFieldAccessor(ref ____obstaclePrefab) = _visualWrappersOriginal;
            
            _visualWrappersOriginal = null;
            
            return;
        }

        if(!BetterBeatSaberConfig.Instance.TransparentObstacles)
            return;

        _visualWrappersOriginal = VisualWrappersFieldAccessor(ref ____obstaclePrefab);
        if(_visualWrappersOriginal?.Length != 2)
            return;

        VisualWrappersFieldAccessor(ref ____obstaclePrefab) = new[] { _visualWrappersOriginal[1] };
        
        _visualWrappersOriginal[0].SetActive(false);
        
        #endregion

        #region Bomb Colors

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

        #endregion
        
    }

}