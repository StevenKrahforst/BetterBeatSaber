using BeatSaberMarkupLanguage.GameplaySetup;

namespace BetterBeatSaber.UI.ModifierView;

public sealed class ModifierView {

    private static ModifierView? _instance;
    public static ModifierView Instance {
        get {
            _instance ??= new ModifierView();
            return _instance;
        }
    }
    
    public bool HideLevelEnvironment {
        get => BetterBeatSaberConfig.Instance.HideLevelEnvironment.CurrentValue;
        set => BetterBeatSaberConfig.Instance.HideLevelEnvironment.SetValue(value);
    }
    
    public bool HideMenuEnvironment {
        get => BetterBeatSaberConfig.Instance.HideMenuEnvironment.CurrentValue;
        set => BetterBeatSaberConfig.Instance.HideMenuEnvironment.SetValue(value);
    }
    
    public static void Initialize() =>
        GameplaySetup.instance.AddTab("Better Beat Saber", "BetterBeatSaber.UI.ModifierView.ModifierView.bsml", Instance);

}