using UnityEngine;

namespace BetterBeatSaber.Utilities;

// ReSharper disable StaticMemberInGenericType

// https://discussions.unity.com/t/create-a-persistent-gameobject-using-singleton/86973/4
// Old HMLib code
// Because of 1.29.1+ ...
public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T? _instance;
    
    private static readonly object Lock = new();
    private static bool _applicationIsQuitting;

    public static T Instance {
        get {
            
            if (_applicationIsQuitting) // This would obv throw an NullPointerException
                return default!;
            
            lock (Lock) {
                
                if (_instance != null)
                    return _instance;
                
                _instance = (T) FindObjectOfType(typeof(T));
                if (FindObjectsOfType(typeof(T)).Length > 1)
                    return _instance;

                if (_instance != null)
                    return _instance;
                    
                var gameObject = new GameObject(typeof(T).ToString());
                _instance = gameObject.AddComponent<T>();
                DontDestroyOnLoad(gameObject);
                
                return _instance;
            }
            
        }
    }

    public static bool IsSingletonAvailable => !_applicationIsQuitting && _instance != null;

    public virtual void OnEnable() => DontDestroyOnLoad(this);

    protected virtual void OnDestroy() => _applicationIsQuitting = true;
    
}