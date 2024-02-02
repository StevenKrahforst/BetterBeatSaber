using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin("Custom Notes", "CustomNotes.Managers.CustomNoteController")]
internal static class CustomNoteControllerMixin {

    [MixinMethod(nameof(Visuals_DidInit), MixinAt.Post)]
    private static void Visuals_DidInit(GameObject ___activeNote) {

        var outline = ___activeNote.gameObject.GetComponent<Outline>();
        switch (BetterBeatSaberConfig.Instance.ColorizeCustomNoteOutlines) {
            case true when outline == null:
                outline = ___activeNote.gameObject.AddComponent<Outline>();
                outline.Width = BetterBeatSaberConfig.Instance.NoteOutlines.Width;
                outline.Visibility = BetterBeatSaberConfig.Instance.NoteOutlines.Visibility;
                outline.Bloom = BetterBeatSaberConfig.Instance.NoteOutlines.Bloom;
                break;
            case false when outline != null:
                Object.Destroy(outline);
                break;
        }
        
    }

}