using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Providers;

public sealed class MaterialProvider : IInitializable {

    public Material? DefaultUIMaterial { get; private set; }
    public Material? DistanceFieldMaterial { get; private set; }
    
    public void Initialize() {
        DefaultUIMaterial = new Material(Shader.Find("UI/Default"));
        DistanceFieldMaterial = new Material(Shader.Find("TextMeshPro/Distance Field"));
    }

}