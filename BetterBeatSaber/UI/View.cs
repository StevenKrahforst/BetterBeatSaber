using System;
using System.Reflection;

using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;

using BetterBeatSaber.Extensions;

using HMUI;

namespace BetterBeatSaber.UI; 

public abstract class View : ViewController {

    public const string Fallback = "<text text='ERROR' />";

    protected static BetterBeatSaber BetterBeatSaber => BetterBeatSaber.Instance;
    
    protected BSMLParserParams? Parameters { get; private set; }
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
        if(firstActivation)
            Parameters = BSMLParser.instance.Parse(ReadDefinition(), gameObject, this);
    }

    [UIAction("#post-parse")]
    public void PostParse() => OnParsed();

    protected virtual void OnParsed() { }

    protected virtual string ReadDefinition() =>
        GetType().ReadDefinition(Fallback);

    #region Create

    public static View CreateView(Type type, bool useSingletonInstance = true) {
        if (!useSingletonInstance)
            return (View) CreateViewController(type)!;
        var instance = type.GetInstance<View>();
        return instance != null ? instance : (View) CreateViewController(type)!;
    }

    internal static ViewController? CreateViewController(Type type) {
        var viewController =(ViewController?)typeof(BeatSaberUI)
            .GetMethod(nameof(BeatSaberUI.CreateViewController), BindingFlags.Public | BindingFlags.Static)?
            .MakeGenericMethod(type)
            .Invoke(null, Array.Empty<object>());
        DontDestroyOnLoad(viewController);
        return viewController;
    }

    #endregion

}

public abstract class View<T> : View where T : View<T> {

    private static T? _instance;
    public static T Instance {
        get {
            _instance ??= (T) CreateViewController(typeof(T))!;
            return _instance;
        }
    }

}