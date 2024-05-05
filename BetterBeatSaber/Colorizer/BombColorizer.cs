using UnityEngine;

namespace BetterBeatSaber.Colorizer; 

internal sealed class BombColorizer : MonoBehaviour {

    private static readonly int SimpleColor = Shader.PropertyToID("_SimpleColor");
    
    private Material[]? _sharedMaterials;

    private void Start() =>
        _sharedMaterials = gameObject.GetComponentInChildren<Renderer>().sharedMaterials;

    private void Update() {

        if (_sharedMaterials == null || !BetterBeatSaberConfig.Instance.ColorizeBombs)
            return;
        
        foreach (var sharedMaterial in _sharedMaterials)
            sharedMaterial.SetColor(SimpleColor, Manager.ColorManager.Instance.FirstColor);

    }

}