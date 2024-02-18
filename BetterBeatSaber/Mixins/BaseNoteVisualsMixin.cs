using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(BaseNoteVisuals))]
internal static class BaseNoteVisualsMixin {

    [MixinMethod(nameof(Awake), MixinAt.Post)]
    private static void Awake(NoteController ____noteController) {
        switch (____noteController) {
            case GameNoteController when BetterBeatSaberConfig.Instance.NoteOutlines.Enable:
                AddOutlines(____noteController, BetterBeatSaberConfig.Instance.NoteOutlines);
                break;
            case BombNoteController when BetterBeatSaberConfig.Instance.BombOutlines.Enable:
                AddOutlines(____noteController, BetterBeatSaberConfig.Instance.BombOutlines);
                break;
        }
    }
    
    [MixinMethod(nameof(OnDestroy), MixinAt.Post)]
    // ReSharper disable once SuggestBaseTypeForParameter
    private static void OnDestroy(NoteController ____noteController) {
        var outline = ____noteController.gameObject.GetComponent<Outline>();
        if (outline != null)
            Object.Destroy(outline);
    }
    
    // ReSharper disable once SuggestBaseTypeForParameter
    private static void AddOutlines(NoteController noteController, Outline.Config outlineConfig) {

        var outline = noteController.gameObject.GetComponent<Outline>();
        if (outline == null)
            outline = noteController is GameNoteController
                ? noteController.gameObject.AddComponent<NoteOutline>()
                : noteController.gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.Width = outlineConfig.Width;
        outline.Bloom = outlineConfig.Bloom;
        outline.Visibility = outlineConfig.Visibility;
        outline.RGB = true;
        
    }

}