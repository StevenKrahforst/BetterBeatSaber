using BeatSaberMarkupLanguage;

using UnityEngine;

namespace BetterBeatSaber.Extensions; 

public static class GameObjectExtensions {

    public static T AddComponent<T>(this GameObject gameObject, T toAdd) where T : Component =>
        gameObject.AddComponent<T>().GetCopyOf(toAdd);

    public static void SetActiveIfNot(this GameObject? gameObject) {
        if(gameObject != null && !gameObject.activeSelf)
            gameObject.SetActive(true);
    }
    
    public static void SetInactiveIfNot(this GameObject? gameObject) {
        if(gameObject != null && gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public static void SetActiveIf(this GameObject? gameObject, bool when, bool setInactiveIfNot = true) {

        if (gameObject == null)
            return;
        
        switch (when) {
            case true when !gameObject.activeSelf:
                gameObject.SetActive(true);
                break;
            case false when gameObject.activeSelf && setInactiveIfNot:
                gameObject.SetActive(false);
                break;
        }
        
    }
    
}