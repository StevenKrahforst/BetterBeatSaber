using System.Linq;

using BetterBeatSaber.Enums;
using BetterBeatSaber.Extensions;

using UnityEngine;
using UnityEngine.Rendering;

namespace BetterBeatSaber.Utilities; 

// TODO: Update
public sealed class Outline : MonoBehaviour {

    private static readonly int ColorMaskProperty = Shader.PropertyToID("_ColorMask");
    
    private static readonly int VisibilityProperty = Shader.PropertyToID("_Visibility");
    
    private static readonly int ColorProperty = Shader.PropertyToID("_OutlineColor");
    private static readonly int Color1Property = Shader.PropertyToID("_OutlineColor1");
    private static readonly int WidthProperty = Shader.PropertyToID("_OutlineWidth");

    private static readonly int ZTest = Shader.PropertyToID("_ZTest");
    
    private const ColorWriteMask GlowingMask = ColorWriteMask.All;
    private const ColorWriteMask GlowingDisabledMask = ColorWriteMask.All & ~ColorWriteMask.Alpha;
    
    internal static Material OutlineMaskMaterialSource = null!;
    internal static Material OutlineFillMaterialSource = null!;

    private bool _needsUpdate;
    
    private Visibility _visibility = Visibility.Everywhere;
    public Visibility Visibility {
        get => _visibility;
        set {
            _visibility = value;
            _needsUpdate = true;
        }
    }
    
    private Color _color = Color.white.WithAlpha(0f);
    public Color Color {
        get => _color;
        set {
            _color = value;
            _needsUpdate = true;
        }
    }
    
    private float _width = 4f;
    public float OutlineWidth {
        get => _width;
        set {
            _width = value;
            _needsUpdate = true;
        }
    }

    public bool RGB { get; set; } = true;

    private bool _glowing = true;
    public bool Glowing {
        get => _glowing;
        set {
            _glowing = value;
            _needsUpdate = true;
        }
    }
    
    private MeshRenderer[]? _renderers;
    
    private Material _maskMaterial = null!;
    private Material _fillMaterial = null!;
    
    private void Awake() {

        _renderers = GetComponentsInChildren<MeshRenderer>();
        if (_renderers.Length > 1)
            _renderers = _renderers.Where(x => x.gameObject.name != "NoteArrowGlow" && x.gameObject.name != "NoteCircleGlow").ToArray();
        
        _maskMaterial = Instantiate(OutlineMaskMaterialSource);
        _fillMaterial = Instantiate(OutlineFillMaterialSource);

        _maskMaterial.name = "OutlineMask (Instance)";
        _fillMaterial.name = "OutlineFill (Instance)";

        _needsUpdate = true;

    }
    
    private void Update() {
        
        if (!_needsUpdate)
            return;
        
        _needsUpdate = false;
        
        UpdateMaterialProperties();
        
    }
    
    private void FixedUpdate() {
        
        if (!RGB)
            return;
        
        _fillMaterial.SetColor(ColorProperty, Utilities.RGB.Instance.FirstColor);
        _fillMaterial.SetColor(Color1Property, Utilities.RGB.Instance.SecondColor);
        
    }

    private void OnEnable() {
        
        if (_renderers == null)
            return;
        
        foreach (var renderer in _renderers) {
            var materials = renderer.sharedMaterials.ToList();
            materials.Add(_maskMaterial);
            materials.Add(_fillMaterial);
            renderer.materials = materials.ToArray();
        }
        
    }

    private void OnDisable() {
        
        if (_renderers == null)
            return;
        
        foreach (var renderer in _renderers) {
            var materials = renderer.sharedMaterials.ToList();
            materials.Remove(_maskMaterial);
            materials.Remove(_fillMaterial);
            renderer.materials = materials.ToArray();
        }
        
    }

    private void OnDestroy() {
        Destroy(_maskMaterial);
        Destroy(_fillMaterial);
    }

    private void UpdateMaterialProperties() {
        
        _fillMaterial.SetInt(VisibilityProperty, (int) _visibility);
        
        if(!RGB)
            _fillMaterial.SetColor(ColorProperty, _color);
        
        _fillMaterial.SetFloat(WidthProperty, _width);
        _fillMaterial.SetFloat(ColorMaskProperty, (float) (_glowing ? GlowingMask : GlowingDisabledMask));
        _maskMaterial.SetFloat(ZTest, (float) CompareFunction.Always);
        _fillMaterial.SetFloat(ZTest, (float) CompareFunction.Always);
        
    }

    public class OutlineConfig {

        public ObservableValue<bool> Enable { get; set; } = true;
        public ObservableValue<bool> Bloom { get; set; } = true;
        public ObservableValue<float> Width { get; set; } = 3f;
        public Visibility Visibility { get; set; } = Visibility.Desktop;

    }

}