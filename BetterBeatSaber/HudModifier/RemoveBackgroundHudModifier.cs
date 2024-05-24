using UnityEngine;

namespace BetterBeatSaber.HudModifier; 

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class RemoveBackgroundHudModifier : IHudModifier {

    public void Initialize() {
        
        var leftPanelBackground = GameObject.Find("LeftPanel/BG");
        if (leftPanelBackground != null)
            Object.DestroyImmediate(leftPanelBackground);
        
        var rightPanelBackground = GameObject.Find("RightPanel/BG");
        if (rightPanelBackground != null)
            Object.DestroyImmediate(rightPanelBackground);
        
    }

}