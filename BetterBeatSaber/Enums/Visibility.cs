using System;

namespace BetterBeatSaber.Enums; 

[Flags]
public enum Visibility : byte {

    Everywhere = VR | Desktop,
    VR = 0x01,
    Desktop = 0x02

}