using BeatSaberMarkupLanguage.GameplaySetup;

using BetterBeatSaber.Extensions;

namespace BetterBeatSaber.UI;

public abstract class ModifierView {

    protected BetterBeatSaber BetterBeatSaber => BetterBeatSaber.Instance;
    
    public abstract string Title { get; }
    public virtual MenuType MenuType { get; } = MenuType.All;

}

public abstract class ModifierView<T> : ModifierView where T : ModifierView<T> {

    private static T? _instance;
    public static T Instance {
        get {
            _instance ??= (T?) typeof(T).Construct();
            return _instance!;
        }
    }

}