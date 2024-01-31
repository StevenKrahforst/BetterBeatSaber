using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

using HMUI;

namespace BetterBeatSaber.UI.Main;

public partial class MainView : BSMLAutomaticViewController {

    private static MainView? _instance;

    public static MainView Instance {
        get {
            _instance ??= BeatSaberUI.CreateViewController<MainView>();
            return _instance;
        }
    }
    
    [UIAction(nameof(OnTabSelected))]
    private void OnTabSelected(SegmentedControl _, int __) {}
    
    /*[UIAction(nameof(OnTabSelected))]
    // ReSharper disable once UnusedMember.Local
    private void OnTabSelected(SegmentedControl _, int index) {
        if (index == 4)
            CreateNotes();
        if (index == 5)
            CreateBomb();
        else {
            DestroyNotes();
            DestroyBomb();
        }
    }

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
        base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        GetTemplates();
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling) {
        base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        DestroyNotes();
    }*/

}