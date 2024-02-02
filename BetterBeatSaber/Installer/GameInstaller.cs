﻿using System;
using System.Collections.Generic;

using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Discord;
using BetterBeatSaber.Extensions;
using BetterBeatSaber.HudModifier;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

using Object = UnityEngine.Object;

namespace BetterBeatSaber.Installer; 

public sealed class GameInstaller : Zenject.Installer {

    //childTransform.gameObject.name == "DustPS"???
    private static readonly List<string> GameObjectNamesToIgnore = [
        "PlayersPlace",
        "DustPS"
    ];
    
    public override void InstallBindings() {

        Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayersPlaceColorizer>().AsSingle();

        BindHudModifier<ComboHudModifier>();
        BindHudModifier<EnergyHudModifier>();
        BindHudModifier<MultiplierHudModifier>();
        BindHudModifier<ProgressHudModifier>();
        BindHudModifier<RemoveBackgroundHudModifier>();
        BindHudModifier<ScoreHudModifier>();

        // TODO: Update
        Container.BindInterfacesAndSelfTo<EnvironmentHider>().AsSingle();
        
        if (!BetterBeatSaberConfig.Instance.HideLevelEnvironment)
            return;
        
        for (var i = 0; i < SceneManager.sceneCount; i++) {
            
            //Resources.FindObjectsOfTypeAll<PlayerData>().FirstOrDefault()
            //    .
            
            var scene = SceneManager.GetSceneAt(i);
            if (!BetterBeatSaberConfig.Instance.HideLevelEnvironment)
                continue;

            if (scene.name == "GameCore") {
                foreach (var gameObject in scene.GetRootGameObjects())
                    if (gameObject.name.Contains("Ring"))
                        Object.Destroy(gameObject);
            } else if (scene.name.Contains("Environment")) {
                var environment = scene.GetRootGameObjects()[0];
                var environmentTransform = environment.GetComponent<Transform>();
                for (i = 2; i < environmentTransform.childCount; i++) {
                    var childTransform = environmentTransform.GetChild(i);
                    if(childTransform.gameObject.name == "PlayersPlace" || childTransform.gameObject.name == "DustPS" || childTransform.gameObject.name.Contains("GameHUD"))
                        continue;
                    Object.Destroy(childTransform.gameObject);
                }
            }
            
        }

    }
    
    private void BindHudModifier<T>() where T : HudModifier.HudModifier =>
        Container.BindInterfacesAndSelfTo<T>().AsSingle();

    private sealed class EnvironmentHider : IInitializable {

        [Inject]
        private readonly GameplayCoreSceneSetupData _setupData;

        [Inject]
        private readonly DiscordManager _discordManager;
        
        public void Initialize() {
            _discordManager.UpdateCurrentActivity(_setupData.difficultyBeatmap.level.songName, largeImageKey: $"https://cdn.beatsaver.com/{_setupData.difficultyBeatmap.level.GetHash().ToLower()}.jpg");
            Console.WriteLine("L: " + _setupData.difficultyBeatmap.level.levelID);
        }

    }
    
}