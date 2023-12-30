using System.Reflection;

using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Colorizer;

internal sealed class BurnMarkAreaColorizer : MonoBehaviour {

    public SaberBurnMarkArea? saberBurnMarkArea;
    
    private LineRenderer[]? _lineRenderers;
    
    private void Start() {
        if (saberBurnMarkArea != null)
            _lineRenderers = (LineRenderer[]?) typeof(SaberBurnMarkArea).GetField("_lineRenderers", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(saberBurnMarkArea);
    }

    private void Update() {
        
        if (_lineRenderers == null)
            return;
        
        foreach (var renderer in _lineRenderers) {
            renderer.startColor = RGB.Instance.FirstColor;
            renderer.endColor = RGB.Instance.SecondColor;
        }
        
    }

}