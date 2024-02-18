using UnityEngine.UI;

namespace BetterBeatSaber.Online.Extensions; 

public static class ButtonExtensions {

    public static void SetInteractableIfNot(this Button button) {
        if (!button.interactable)
            button.interactable = true;
    }

}