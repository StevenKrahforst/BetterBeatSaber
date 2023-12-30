using UnityEngine;

using Zenject;

namespace BetterBeatSaber.HudModifier; 

internal sealed class RemoveBackgroundHudModifier : HudModifier, IInitializable {

    public void Initialize() {
        
        var leftPanelBackground = GameObject.Find("LeftPanel/BG");
        if (leftPanelBackground != null) {
            Object.DestroyImmediate(leftPanelBackground);
        }
        
        var rightPanelBackground = GameObject.Find("RightPanel/BG");
        if (rightPanelBackground != null) {
            Object.DestroyImmediate(rightPanelBackground);
        }
        
    }

}