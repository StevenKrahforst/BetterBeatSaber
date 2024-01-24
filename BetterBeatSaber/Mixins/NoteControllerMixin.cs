using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(NoteController))]
internal static class NoteControllerMixin {

    [MixinMethod(nameof(Init), MixinAt.Post)]
    private static void Init(ref NoteController __instance) {
        // ReSharper disable once Unity.InefficientPropertyAccess
        __instance.gameObject.transform.localScale = __instance switch {
            GameNoteController => BetterBeatSaberConfig.Instance.NoteSize * Vector3.one,
            BombNoteController => BetterBeatSaberConfig.Instance.BombSize * Vector3.one,
            _ => __instance.gameObject.transform.localScale
        };
    }

}