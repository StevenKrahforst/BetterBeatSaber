using System;

namespace BetterBeatSaber.Mixin.Enums;

[Flags]
public enum MixinAction : byte {

    All = Register | Patch | Unpatch | Toggle,
    Register = 0x2,
    Patch = 0x4,
    Unpatch = 0x8,
    Toggle = 0x16

}