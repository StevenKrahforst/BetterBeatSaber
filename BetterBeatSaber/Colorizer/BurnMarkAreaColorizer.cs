using System.Reflection;

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
        
        if (_lineRenderers == null || Manager.ColorManager.Instance == null)
            return;
        
        foreach (var renderer in _lineRenderers) {
            renderer.startColor = Manager.ColorManager.Instance.FirstColor;
            renderer.endColor = Manager.ColorManager.Instance.SecondColor;
        }
        
    }

}