using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;

using HMUI;

namespace BetterBeatSaber.UI;

internal sealed class MainFlowCoordinator : FlowCoordinator {

    private static MainFlowCoordinator? _instance;

    public static MainFlowCoordinator Instance {
        get {
            _instance ??= BeatSaberUI.CreateFlowCoordinator<MainFlowCoordinator>();
            return _instance;
        }
    }
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
        if (firstActivation) {
            SetTitle("Better Beat Saber");
            showBackButton = true;
        }
        ProvideInitialViewControllers(Main.MainView.Instance);
    }
    
    protected override void BackButtonWasPressed(ViewController _) {
        BetterBeatSaberConfig.Instance.Save();
        BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
    }

    private static MenuButton? _menuButton;
    
    internal static void Initialize() {
        _menuButton ??= new MenuButton("Better Beat Saber", "Better Beat Saber", Show);
        MenuButtons.instance.RegisterButton(_menuButton);
        ModifierView.ModifierView.Initialize();
    }

    private static void Show() =>
        BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(Instance);

}