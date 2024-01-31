using System;
using System.Collections.Generic;

using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Discord;

using Discord;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Installer; 

public sealed class MenuInstaller : Zenject.Installer, IDisposable {

    [Inject]
    private readonly BetterBeatSaber _betterBeatSaber = null!;
    
    [Inject]
    private readonly DiscordManager _discordManager = null!;
    
    public override void InstallBindings() {
        
        //if (BetterBeatSaberConfig.Instance.ColorizeDust)
        //    Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        
        if (BetterBeatSaberConfig.Instance.ColorizeFeet)
            Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();
        
        //if(BetterBeatSaberConfig.Instance.ColorizeMenuSign)
        //    Container.BindInterfacesAndSelfTo<MenuSignColorizer>().AsSingle();

        LoadMenuGameObjects();

        if (BetterBeatSaberConfig.Instance.HideMenuEnvironment)
            SetMenuEnvironment(false);
        
        BetterBeatSaberConfig.Instance.HideMenuEnvironment.OnValueChanged += HideMenuEnvironmentOnOnValueChanged;

        Console.WriteLine("AAAAAA");
        
        _discordManager.UpdateActivity(new Activity {
            Name = "Beat Saber",
            Details = "Relaxing in the Menu",
            State = "UwU",
            Assets = new ActivityAssets {
                LargeImage = "menu",
                LargeText = "In Menu",
                SmallImage = "logo",
                SmallText = $"Better Beat Saber v{_betterBeatSaber.Version}"
            }
        });

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