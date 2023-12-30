using HMUI;

namespace BetterBeatSaber.Extensions; 

public static class TextSegmentedControlExtensions {

    public static void SetInteractable(this TextSegmentedControl textSegmentedControl, bool interactable) {
        foreach (var cell in textSegmentedControl.cells)
            cell.interactable = interactable;
    }

}