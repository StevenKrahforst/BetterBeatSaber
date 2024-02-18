using UnityEngine;

namespace BetterBeatSaber.Colorizer; 

internal sealed class BombColorizer : MonoBehaviour {

    private static readonly int SimpleColor = Shader.PropertyToID("_SimpleColor");
    
    private Material? _sharedMaterial;

    private void Start() =>
        _sharedMaterial = gameObject.GetComponentInChildren<Renderer>().sharedMaterial;

    private void Update() {

        if (_sharedMaterial == null || !BetterBeatSaberConfig.Instance.Bombs.Colorize || Manager.ColorManager.Instance == null)
            return;

        _sharedMaterial.SetColor(SimpleColor, Manager.ColorManager.Instance.FirstColor);

    }

}