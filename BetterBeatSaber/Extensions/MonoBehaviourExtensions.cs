using UnityEngine;

namespace BetterBeatSaber.Extensions; 

public static class MonoBehaviourExtensions {

    public static void SetActiveIfNot(this MonoBehaviour? monoBehaviour) =>
        // ReSharper disable once Unity.NoNullPropagation
        monoBehaviour?.gameObject.SetActiveIfNot();

    public static void SetInactiveIfNot(this MonoBehaviour? monoBehaviour) =>
        // ReSharper disable once Unity.NoNullPropagation
        monoBehaviour?.gameObject.SetInactiveIfNot();

    public static void SetActiveIf(this MonoBehaviour? monoBehaviour, bool when, bool setInactiveIfNot = true) =>
        // ReSharper disable once Unity.NoNullPropagation
        monoBehaviour?.gameObject.SetActiveIf(when, setInactiveIfNot);

}