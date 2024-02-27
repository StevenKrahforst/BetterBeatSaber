using System.Collections.Generic;
using System.Linq;

using BetterBeatSaber.Enums;

using UnityEngine;
using UnityEngine.Rendering;

// Heavily modified version of https://github.com/chrisnolet/QuickOutline

namespace BetterBeatSaber.Utilities;

internal class Outline : MonoBehaviour {

    #region Shader IDs

    private static readonly int ZTestProperty = Shader.PropertyToID("_ZTest");
    private static readonly int ColorMaskProperty = Shader.PropertyToID("_ColorMask");
    private static readonly int VisibilityProperty = Shader.PropertyToID("_Visibility");
    private static readonly int FirstColorProperty = Shader.PropertyToID("_FirstColor");
    private static readonly int SecondColorProperty = Shader.PropertyToID("_SecondColor");
    private static readonly int WidthProperty = Shader.PropertyToID("_Width");
        
    #endregion

    #region Constants

    private const ColorWriteMask BloomMask = ColorWriteMask.All;
    private const ColorWriteMask NoBloomMask = BloomMask & ~ColorWriteMask.Alpha;

    #endregion
        
    #region Materials

    internal static Material MaskMaterial = null!;
    internal static Material FillMaterial = null!;

    #endregion

    public enum Mode {

        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly

    }

    #region Fields

    private bool _needsUpdate;
    protected IEnumerable<MeshRenderer> Renderers = Enumerable.Empty<MeshRenderer>();

    #endregion
        
    #region Properties

    private Mode _mode;
    public Mode OutlineMode {
        get => _mode;
        set {
                
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (value) {
                case Mode.OutlineAll:
                    _maskMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.Always);
                    _fillMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.Always);
                    break;
                case Mode.OutlineVisible:
                    _maskMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.Always);
                    _fillMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.LessEqual);
                    break;
                case Mode.OutlineHidden:
                    _maskMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.Always);
                    _fillMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.Greater);
                    break;
                case Mode.OutlineAndSilhouette:
                    _maskMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.LessEqual);
                    _fillMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.Always);
                    break;
                case Mode.SilhouetteOnly:
                    _maskMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.LessEqual);
                    _fillMaterialPropertyBlock.SetFloat(ZTestProperty, (float) CompareFunction.Greater);
                    Width = 0f;
                    break;
            }

            _mode = value;
            
            _needsUpdate = true;
                
        }
    }

    public ColorWriteMask ColorMask {
        get => (ColorWriteMask) _fillMaterialPropertyBlock.GetFloat(ColorMaskProperty);
        set {
            _fillMaterialPropertyBlock.SetFloat(ColorMaskProperty, (float)value);
            _needsUpdate = true;
        }
    }

    public Visibility Visibility {
        get => (Visibility) _fillMaterialPropertyBlock.GetInt(VisibilityProperty);
        set {
            _fillMaterialPropertyBlock.SetInt(VisibilityProperty, (int) value);
            _needsUpdate = true;
        }
    }

    public Color Color {
        set {
            FirstColor = value;
            SecondColor = value;
        }
    }
        
    public Color FirstColor {
        get => _fillMaterialPropertyBlock.GetColor(FirstColorProperty);
        set {
            _fillMaterialPropertyBlock.SetColor(FirstColorProperty, value);
            _needsUpdate = true;
        }
    }
        
    public Color SecondColor {
        get => _fillMaterialPropertyBlock.GetColor(SecondColorProperty);
        set {
            _fillMaterialPropertyBlock.SetColor(SecondColorProperty, value);
            _needsUpdate = true;
        }
    }

    public float Width {
        get => _fillMaterialPropertyBlock.GetFloat(WidthProperty);
        set {
            _fillMaterialPropertyBlock.SetFloat(WidthProperty, value);
            _needsUpdate = true;
        }
    }
    
    #endregion

    #region Beat Saber

    public bool Bloom {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        get => _fillMaterialPropertyBlock.GetFloat(ColorMaskProperty) == (float) BloomMask;
        set {
            _fillMaterialPropertyBlock.SetFloat(ColorMaskProperty, (float) (value ? BloomMask : NoBloomMask));
            _needsUpdate = true;
        }
    }

    public bool RGB { get; set; }

    #endregion

    #region Static Properties
    
    private MaterialPropertyBlock _maskMaterialPropertyBlock = null!;
    private MaterialPropertyBlock _fillMaterialPropertyBlock = null!;

    #endregion
    
    private void Awake() {
        
        _maskMaterialPropertyBlock = new MaterialPropertyBlock();
        _fillMaterialPropertyBlock = new MaterialPropertyBlock();

        UpdateRenderers();
        
        UpdateMaterialProperties();

    }

    private void Update() {
        
        if (RGB) {
            FirstColor = Manager.ColorManager.Instance!.FirstColor;
            SecondColor = Manager.ColorManager.Instance.SecondColor;
        }
        
        if (_needsUpdate)
            UpdateMaterialProperties();
        
    }

    private void OnEnable() {
        foreach (var r in Renderers) {
            
            var materials = r.sharedMaterials.ToList();
            
            materials.Add(MaskMaterial);
            materials.Add(FillMaterial);
            
            r.materials = materials.ToArray();
            
        }
    }

    private void OnDisable() {
        foreach (var r in Renderers) {
            
            var materials = r.sharedMaterials.ToList();
            
            materials.Remove(MaskMaterial);
            materials.Remove(FillMaterial);
            
            r.materials = materials.ToArray();
            
        }
    }
        
    private void OnDestroy() {
        foreach (var r in Renderers)
            for (var i = 0; i < r.sharedMaterials.Length; i++)
                r.SetPropertyBlock(null, i);
    }

    private void UpdateMaterialProperties() {
        if(_needsUpdate)
            _needsUpdate = false;
        foreach (var r in Renderers) {
            for (var i = 0; i < r.sharedMaterials.Length; i++) {
                var sharedMaterial = r.sharedMaterials[i];
                if(sharedMaterial == MaskMaterial)
                    r.SetPropertyBlock(_maskMaterialPropertyBlock, i);
                else if(sharedMaterial == FillMaterial)
                    r.SetPropertyBlock(_fillMaterialPropertyBlock, i);
            }
        }
    }

    protected virtual void UpdateRenderers() =>
        Renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
    
    public class Config {

        public ObservableValue<bool> Enable { get; set; } = true;
        public ObservableValue<bool> Bloom { get; set; } = true;
        public ObservableValue<float> Width { get; set; } = 3f;
        public Visibility Visibility { get; set; } = Visibility.Desktop;

    }

}