using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterBeatSaber.Utilities; 

public class EnvironmentHider {

    internal static void HideEnvironment() {
        for (var i = 0; i < SceneManager.sceneCount; i++) {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name == "MainMenu") {
                //LoadMenuGameObjects();
                //SetMenuEnvironment(!DarkEnvironmentConfig.Instance.HideMenuEnvironment);
            } else if (BetterBeatSaberConfig.Instance.HideLevelEnvironment) {
                HideLevelEnvironment(scene);
            }
        }
    }

    #region Level Environment
    
    private static void HideLevelEnvironment(Scene scene) {

        if (scene.name == "GameCore") {
            MenuGameObjects.Clear();
            foreach (var gameObject in scene.GetRootGameObjects()) {
                if (gameObject.name.Contains("Ring")) {
                    Object.Destroy(gameObject);
                }
            }
        }

        if (scene.name.Contains("Environment")) {
            var environment = scene.GetRootGameObjects()[0];
            var environmentTransform = environment.GetComponent<Transform>();
            for (var i = 2; i < environmentTransform.childCount; i++) {
                var childTransform = environmentTransform.GetChild(i);
                //childTransform.gameObject.name == "DustPS"
                if(childTransform.gameObject.name == "PlayersPlace" || childTransform.gameObject.name.Contains("GameHUD"))
                    continue;
                Object.Destroy(childTransform.gameObject);
            }
        }
        
    }

    #endregion

    #region Menu Environment

    private static readonly List<string> MenuGameObjectsName = new() {
        "MenuFogRing",
        "BackgroundGradient",
        "BasicMenuGround",
        "Notes",
        "PileOfNotes"
    };
    
    private static readonly List<GameObject> MenuGameObjects = new();

    internal static void LoadMenuGameObjects() {
        MenuGameObjects.Clear();
        foreach (var gameObjectName in MenuGameObjectsName) {
            var gameObject = GameObject.Find(gameObjectName);
            if(gameObject != null)
                MenuGameObjects.Add(gameObject);
        }
    }
    
    internal static void SetMenuEnvironment(bool value) {
        foreach (var gameObject in MenuGameObjects) {
            gameObject.SetActive(value);
        }
    }

    #endregion


}