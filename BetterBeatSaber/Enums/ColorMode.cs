using System;

namespace BetterBeatSaber.Enums;

[Flags]
public enum ColorMode : byte {

    Gradient = 2,
    
    RGB = 4,
    RGBGradient = RGB | Gradient,
    
    Color = 8,
    ColorGradient = Color | Gradient

}