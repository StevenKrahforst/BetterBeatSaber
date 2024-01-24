using BetterBeatSaber.Mixin;
using BetterBeatSaber.Mixin.Attributes;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(SceneManager))]
internal static class SceneManagerMixin {

    public static GameObject? DefaultNote;
    
    [MixinMethod("Internal_ActiveSceneChanged", MixinAt.Pre)]
    private static void Internal_ActiveSceneChanged(ref Scene newActiveScene) {
        
        if (!newActiveScene.IsValid() || newActiveScene.name != "ShaderWarmup")
            return;
        
        DefaultNote = Object.Instantiate(GameObject.Find("NormalGameNote"));
        DefaultNote.SetActive(false);
        
        Object.DontDestroyOnLoad(DefaultNote);
        
        var transform = DefaultNote.transform.Find("NoteCube");
        transform.Find("NoteArrow");
        transform.Find("NoteArrowGlow");
        transform.Find("NoteCircleGlow");
        
    }

}