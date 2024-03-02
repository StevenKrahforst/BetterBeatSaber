using System;
using System.Collections.Generic;

using IPA.Loader;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Bindings;

internal sealed class MenuEnvironmentHider : IInitializable, IDisposable {

    private static readonly List<GameObject> MenuGameObjects = [];
    
    private static bool IsAdaptableNotesInstalled => PluginManager.GetPluginFromId("AdaptableNotes") != null;
    private static readonly List<string> AdaptableNotesIgnoredGameObjects = [
        "Notes",
        "PileOfNotes"
    ];
    
    public void Initialize() {
        LoadMenuGameObjects();
        SetMenuEnvironment(!BetterBeatSaberConfig.Instance.HideMenuEnvironment.CurrentValue);
        BetterBeatSaberConfig.Instance.HideMenuEnvironment.OnValueChanged += OnHideMenuEnvironmentChanged;
    }
    
    public void Dispose() =>
        BetterBeatSaberConfig.Instance.HideMenuEnvironment.OnValueChanged -= OnHideMenuEnvironmentChanged;
    
    private static void OnHideMenuEnvironmentChanged(bool state) =>
        SetMenuEnvironment(!state);
    
    private static void LoadMenuGameObjects() {
        
        MenuGameObjects.Clear();
        
        foreach (var gameObjectName in BetterBeatSaberConfig.Instance.MenuGameObjects) {
            
            if (IsAdaptableNotesInstalled && AdaptableNotesIgnoredGameObjects.Contains(gameObjectName))
                continue;
            
            var gameObject = GameObject.Find(gameObjectName);
            if(gameObject != null)
                MenuGameObjects.Add(gameObject);
            
        }
        
    }
    
    private static void SetMenuEnvironment(bool value) {
        foreach (var gameObject in MenuGameObjects)
            gameObject.SetActive(value);
    }

}