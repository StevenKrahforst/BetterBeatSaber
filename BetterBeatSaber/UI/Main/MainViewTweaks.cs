﻿namespace BetterBeatSaber.UI.Main;

public partial class MainView {

    public bool HideMissTexts {
        get => BetterBeatSaberConfig.Instance.HideMissTexts;
        set => BetterBeatSaberConfig.Instance.HideMissTexts = value;
    }
    
    public bool DisableAprilFoolsAndEarthDayStuff {
        get => BetterBeatSaberConfig.Instance.DisableAprilFoolsAndEarthDayStuff;
        set => BetterBeatSaberConfig.Instance.DisableAprilFoolsAndEarthDayStuff = value;
    }
    
    public bool HideEditorButton {
        get => BetterBeatSaberConfig.Instance.HideEditorButton;
        set => BetterBeatSaberConfig.Instance.HideEditorButton = value;
    }
    
    public bool HidePromotionButton {
        get => BetterBeatSaberConfig.Instance.HidePromotionButton;
        set => BetterBeatSaberConfig.Instance.HidePromotionButton = value;
    }
    
    public bool HideLevelEnvironment {
        get => BetterBeatSaberConfig.Instance.HideLevelEnvironment.CurrentValue;
        set => BetterBeatSaberConfig.Instance.HideLevelEnvironment.SetValue(value);
    }
    
    public bool HideMenuEnvironment {
        get => BetterBeatSaberConfig.Instance.HideMenuEnvironment.CurrentValue;
        set => BetterBeatSaberConfig.Instance.HideMenuEnvironment.SetValue(value);
    }
    
}