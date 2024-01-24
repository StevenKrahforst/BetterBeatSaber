using UnityEngine;

namespace BetterBeatSaber.Extensions; 

public static class ColorExtensions {

    public static readonly Color None = new (0f, 0f, 0f, 0f);
    
    public static Color WithAlpha(this Color color, float alpha) {
        color.a = alpha;
        return color;
    }
    
    public static Color LerpHSV(this Color colorA, Color colorB, float t) {
        Color.RGBToHSV(colorA, out var h1, out var s1, out var v1);
        Color.RGBToHSV(colorB, out var h2, out var s2, out var v2);
        return Color.HSVToRGB(Mathf.Lerp(h1, h2, t), Mathf.Lerp(s1, s2, t), Mathf.Lerp(v1, v2, t));
    }

    public static string ToHex(this Color color) =>
        ColorUtility.ToHtmlStringRGB(color);
    
}