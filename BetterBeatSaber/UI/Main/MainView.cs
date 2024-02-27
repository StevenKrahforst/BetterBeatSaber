using System.Collections.Generic;
using System.Linq;

using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

using BetterBeatSaber.Enums;

using HMUI;

using JetBrains.Annotations;

namespace BetterBeatSaber.UI.Main;

internal partial class MainView : BSMLAutomaticViewController {

    private static MainView? _instance;

    public static MainView Instance {
        get {
            _instance ??= BeatSaberUI.CreateViewController<MainView>();
            return _instance;
        }
    }

    [UIAction("#post-parse"), UsedImplicitly]
    private void OnParsed() {
        SetPluginBasedSettingsInteractable();
    }
    
    [UIAction(nameof(OnTabSelected)), UsedImplicitly]
    private void OnTabSelected(SegmentedControl _, int __) {}

    [UIValue(nameof(Visibilities)), UsedImplicitly]
    protected List<object> Visibilities => new[] {
        Visibility.Both,
        Visibility.Desktop,
        Visibility.VR,
        Visibility.None
    }.Select(visibility => visibility.ToString()).Cast<object>().ToList();

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