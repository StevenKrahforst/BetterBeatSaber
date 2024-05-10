using System.Linq;

using BetterBeatSaber.Colorizer;
using BetterBeatSaber.HudModifier;

using UnityEngine;
using UnityEngine.SceneManagement;

using Object = UnityEngine.Object;

namespace BetterBeatSaber.Installer; 

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class GameInstaller : Zenject.Installer {

    public override void InstallBindings() {

        Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayersPlaceColorizer>().AsSingle();

        if(BetterBeatSaberConfig.Instance.ComboHudModifier.Enable)
            BindHudModifier<ComboHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.EnergyHudModifier.Enable)
            BindHudModifier<EnergyHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.MultiplierHudModifier.Enable)
            BindHudModifier<MultiplierHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.ProgressHudModifier.Enable)
            BindHudModifier<ProgressHudModifier>();
        
        BindHudModifier<RemoveBackgroundHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.ScoreHudModifier.Enable)
            BindHudModifier<ScoreHudModifier>();

        if (!BetterBeatSaberConfig.Instance.HideLevelEnvironment)
            return;
        
        for (var i = 0; i < SceneManager.sceneCount; i++) {
            
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
                    if(BetterBeatSaberConfig.Instance.IgnoredLevelGameObjects.Contains(childTransform.gameObject.name) || childTransform.gameObject.name.Contains("GameHUD"))
                        continue;
                    Object.Destroy(childTransform.gameObject);
                }
            }
            
        }

    }
    
    private void BindHudModifier<T>() where T : IHudModifier =>
        Container.BindInterfacesAndSelfTo<T>().AsSingle();

}