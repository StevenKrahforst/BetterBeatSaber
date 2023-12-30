using BetterBeatSaber.Mixin;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[MixinPlugin("Custom Notes", "CustomNotes.Managers.CustomNoteController")]
internal static class CustomNoteControllerMixin {

    [MixinMethod(nameof(Visuals_DidInit), MixinAt.Post)]
    private static void Visuals_DidInit(GameObject ___activeNote) {

        var outline = ___activeNote.gameObject.GetComponent<Outline>();
        switch (BetterBeatSaberConfig.Instance.ColorizeCustomNoteOutlines) {
            case true when outline == null:
                outline = ___activeNote.gameObject.AddComponent<Outline>();
                outline.OutlineWidth = BetterBeatSaberConfig.Instance.NoteOutlines.OutlinesWidth;
                outline.Visibility = BetterBeatSaberConfig.Instance.NoteOutlines.Visibility;
                outline.Glowing = BetterBeatSaberConfig.Instance.NoteOutlines.Glow;
                break;
            case false when outline != null:
                Object.Destroy(outline);
                break;
        }
        
    }

}