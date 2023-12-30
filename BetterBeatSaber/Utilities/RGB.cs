using UnityEngine;

namespace BetterBeatSaber.Utilities;

internal sealed class RGB : PersistentSingleton<RGB> {

    public Color FirstColor { get; private set; }
    public Color SecondColor { get; private set; }
    public Color ThirdColor { get; private set; }

    public float FirstHue { get; private set; }
    public float SecondHue { get; private set; }
    public float ThirdHue { get; private set; }

    private void Update() {
        
        var time = Time.time / 5f;
        var hue = Mathf.Clamp(time % 2f >= 1f ? 1f - time % 1f : time % 1f, .05f, .9f);

        FirstHue = hue;
        SecondHue = Mathf.Max(1f, hue + .05f);
        ThirdHue = Mathf.Max(1f, hue + .15f);
        
        FirstColor = Color.HSVToRGB(FirstHue, 1f, 1f);
        SecondColor = Color.HSVToRGB(SecondHue, 1f, 1f);
        ThirdColor = Color.HSVToRGB(ThirdHue, 1f, 1f);
        
    }

}