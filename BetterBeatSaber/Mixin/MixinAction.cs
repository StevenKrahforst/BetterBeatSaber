using System;

namespace BetterBeatSaber.Mixin;

[Flags]
public enum MixinAction : byte {

    All = Patch | Unpatch | Toggle,
    Patch = 0x2,
    Unpatch = 0x4,
    Toggle = 0x8

}