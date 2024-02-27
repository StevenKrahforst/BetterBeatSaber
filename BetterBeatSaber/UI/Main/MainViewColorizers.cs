using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;

using IPA.Loader;

using JetBrains.Annotations;

namespace BetterBeatSaber.UI.Main;

internal partial class MainView {

    #region Game Colorizers

    public bool ColorizeDust {
        get => BetterBeatSaberConfig.Instance.ColorizeDust.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeDust.SetValue(value);
    }
    
    public bool ColorizeFeet {
        get => BetterBeatSaberConfig.Instance.ColorizeFeet.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeFeet.SetValue(value);
    }
    
    public bool ColorizePlayersPlace {
        get => BetterBeatSaberConfig.Instance.ColorizePlayersPlace.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizePlayersPlace.SetValue(value);
    }
    
    public bool ColorizeBurnMarks {
        get => BetterBeatSaberConfig.Instance.ColorizeBurnMarks.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeBurnMarks.SetValue(value);
    }
    
    public bool ColorizeObstacles {
        get => BetterBeatSaberConfig.Instance.ColorizeObstacles.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeObstacles.SetValue(value);
    }
    
    public bool ColorizeCutParticles {
        get => BetterBeatSaberConfig.Instance.ColorizeCutParticles.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeCutParticles.SetValue(value);
    }
    
    public bool ColorizeArcs {
        get => BetterBeatSaberConfig.Instance.ColorizeArcs.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeArcs.SetValue(value);
    }
    
    public bool ColorizeReeSabers {
        get => BetterBeatSaberConfig.Instance.ColorizeReeSabers.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeReeSabers.SetValue(value);
    }
    
    #endregion

    #region UI Colorizers

    public bool ColorizeButtons {
        get => BetterBeatSaberConfig.Instance.ColorizeButtons.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeButtons.SetValue(value);
    }
    
    public bool ColorizeMenuButtons {
        get => BetterBeatSaberConfig.Instance.ColorizeMenuButtons;
        set => BetterBeatSaberConfig.Instance.ColorizeMenuButtons = value;
    }
    
    // ReSharper disable once InconsistentNaming
    public bool ColorizePBOT {
        get => BetterBeatSaberConfig.Instance.ColorizePBOT;
        set => BetterBeatSaberConfig.Instance.ColorizePBOT = value;
    }
    
    // ReSharper disable once InconsistentNaming
    public bool ColorizeFCPercentage {
        get => BetterBeatSaberConfig.Instance.ColorizeFCPercentage;
        set => BetterBeatSaberConfig.Instance.ColorizeFCPercentage = value;
    }
    
    public bool ColorizeFPSCounter {
        get => BetterBeatSaberConfig.Instance.ColorizeFPSCounter;
        set => BetterBeatSaberConfig.Instance.ColorizeFPSCounter = value;
    }

    public bool ColorizeMenuPillars {
        get => BetterBeatSaberConfig.Instance.ColorizeMenuPillars.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeMenuPillars.SetValue(value);
    }
    
    #endregion

    #region Plugin Based Interactable Settings

    [UIComponent(nameof(ColorizeReeSabersToggle)), UsedImplicitly]
    protected readonly ToggleSetting ColorizeReeSabersToggle = null!;
    
    [UIComponent(nameof(ColorizePBOTToggle)), UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    protected readonly ToggleSetting ColorizePBOTToggle = null!;
    
    [UIComponent(nameof(ColorizeFCPercentageToggle)), UsedImplicitly]
    // ReSharper disable once InconsistentNaming
    protected readonly ToggleSetting ColorizeFCPercentageToggle = null!;
    
    [UIComponent(nameof(ColorizeFPSCounterToggle)), UsedImplicitly]
    protected readonly ToggleSetting ColorizeFPSCounterToggle = null!;
    
    [UIComponent(nameof(ColorizeMenuPillarsToggle)), UsedImplicitly]
    protected readonly ToggleSetting ColorizeMenuPillarsToggle = null!;
    
    private void SetPluginBasedSettingsInteractable() {
        ColorizeReeSabersToggle.interactable = PluginManager.GetPluginFromId("ReeSabers") != null;
        ColorizePBOTToggle.interactable = PluginManager.GetPluginFromId("PBOT") != null;
        ColorizeFCPercentageToggle.interactable = PluginManager.GetPluginFromId("FCPercentage") != null;
        ColorizeFPSCounterToggle.interactable = PluginManager.GetPluginFromId("FPSCounter") != null;
        ColorizeMenuPillarsToggle.interactable = PluginManager.GetPluginFromId("MenuPillars") != null;
    }

    #endregion

}