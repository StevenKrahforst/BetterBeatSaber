using System;

namespace BetterBeatSaber.Enums; 

[Flags]
internal enum Visibility : byte {

    None = 0,
    Desktop = 1, // Desktop Only
    VR = 2, // VR Only
    Both = VR | Desktop

}