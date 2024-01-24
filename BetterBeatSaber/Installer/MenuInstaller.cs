using System;
using System.Collections.Generic;

using BetterBeatSaber.Colorizer;

using UnityEngine;

namespace BetterBeatSaber.Installer; 

public sealed class MenuInstaller : Zenject.Installer, IDisposable {

    public override void InstallBindings() {
        
        if (BetterBeatSaberConfig.Instance.ColorizeDust)
            Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        
        if (BetterBeatSaberConfig.Instance.ColorizeFeet)
            Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();

        LoadMenuGameObjects();

        if (BetterBeatSaberConfig.Instance.HideMenuEnvironment)
            SetMenuEnvironment(false);
        
        BetterBeatSaberConfig.Instance.HideMenuEnvironment.OnValueChanged += HideMenuEnvironmentOnOnValueChanged;
        
    }

    public void Dispose() =>
        BetterBeatSaberConfig.Instance.HideMenuEnvironment.OnValueChanged -= HideMenuEnvironmentOnOnValueChanged;
    
    private static readonly List<string> MenuGameObjectsName = [
        "MenuFogRing",
        "BackgroundGradient",
        "BasicMenuGround",
        "Notes",
        "PileOfNotes"
    ];
    
    private static readonly List<GameObject> MenuGameObjects = [];

    private static void HideMenuEnvironmentOnOnValueChanged(bool state) =>
        SetMenuEnvironment(!state);
    
    private static void LoadMenuGameObjects() {
        MenuGameObjects.Clear();
        foreach (var gameObjectName in MenuGameObjectsName) {
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