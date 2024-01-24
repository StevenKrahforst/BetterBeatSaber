using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Providers;

public sealed class MaterialProvider : IInitializable {

    public static MaterialProvider? Instance { get; private set; }
    
    public Material? DefaultUIMaterial { get; private set; }
    public Material? DistanceFieldMaterial { get; private set; }
    
    public void Initialize() {
        
        Instance = this;
        
        DefaultUIMaterial = new Material(Shader.Find("UI/Default"));
        DistanceFieldMaterial = new Material(Shader.Find("TextMeshPro/Distance Field"));
        
    }

}