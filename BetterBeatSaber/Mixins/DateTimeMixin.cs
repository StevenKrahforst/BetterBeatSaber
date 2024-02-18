using System;

using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixins;

// https://github.com/ItsKaitlyn03/Unfunny/blob/main/Unfunny/HarmonyPatches/DateTime.cs

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(DateTime))]
internal static class DateTimeMixin {

    [MixinMethod(nameof(get_Now), MixinAt.Post)]
    private static void get_Now(ref DateTime __result) {
        if (__result is { Month: 4, Day: 1 or 22 } && BetterBeatSaberConfig.Instance.DisableAprilFoolsAndEarthDayStuff)
            __result = __result.AddDays(1);
    }

}