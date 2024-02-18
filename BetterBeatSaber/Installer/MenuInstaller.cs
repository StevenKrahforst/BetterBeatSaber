using System;
using System.Collections.Generic;

using BetterBeatSaber.Colorizer;

using UnityEngine;

namespace BetterBeatSaber.Installer; 

public sealed class MenuInstaller : Zenject.Installer, IDisposable {

    public override void InstallBindings() {
        
        Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();
        
        if(BetterBeatSaberConfig.Instance.ColorizeMenuSign)
            Container.BindInterfacesAndSelfTo<MenuSignColorizer>().AsSingle();

        LoadMenuGameObjects();

        SetMenuEnvironment(!BetterBeatSaberConfig.Instance.HideMenuEnvironment.CurrentValue);
        
        BetterBeatSaberConfig.Instance.HideMenuEnvironment.OnValueChanged += HideMenuEnvironmentOnOnValueChanged;

    }

    public void Dispose() =>
        BetterBeatSaberConfig.Instance.HideMenuEnvironment.OnValueChanged -= HideMenuEnvironmentOnOnValueChanged;
    
    private static readonly List<GameObject> MenuGameObjects = [];

    private static void HideMenuEnvironmentOnOnValueChanged(bool state) =>
        SetMenuEnvironment(!state);
    
    private static void LoadMenuGameObjects() {
        MenuGameObjects.Clear();
        foreach (var gameObjectName in BetterBeatSaberConfig.Instance.MenuGameObjects) {
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