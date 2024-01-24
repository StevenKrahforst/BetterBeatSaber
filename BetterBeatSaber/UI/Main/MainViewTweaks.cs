namespace BetterBeatSaber.UI.Main;

public partial class MainView {

    public bool HideMissTexts {
        get => BetterBeatSaberConfig.Instance.HideMissTexts;
        set => BetterBeatSaberConfig.Instance.HideMissTexts = value;
    }
    
    public bool DisableCutParticles {
        get => BetterBeatSaberConfig.Instance.DisableCutParticles;
        set => BetterBeatSaberConfig.Instance.DisableCutParticles = value;
    }
    
    public bool DisableBombExplosionEffect {
        get => BetterBeatSaberConfig.Instance.DisableBombExplosionEffect;
        set => BetterBeatSaberConfig.Instance.DisableBombExplosionEffect = value;
    }
    
    public bool DisableAprilFoolsAndEarthDayStuff {
        get => BetterBeatSaberConfig.Instance.DisableAprilFoolsAndEarthDayStuff;
        set => BetterBeatSaberConfig.Instance.DisableAprilFoolsAndEarthDayStuff = value;
    }
    
    public bool DisableComboBreakEffect {
        get => BetterBeatSaberConfig.Instance.DisableComboBreakEffect;
        set => BetterBeatSaberConfig.Instance.DisableComboBreakEffect = value;
    }
    
    public bool HideEditorButton {
        get => BetterBeatSaberConfig.Instance.HideEditorButton;
        set => BetterBeatSaberConfig.Instance.HideEditorButton = value;
    }
    
    public bool HidePromotionButton {
        get => BetterBeatSaberConfig.Instance.HidePromotionButton;
        set => BetterBeatSaberConfig.Instance.HidePromotionButton = value;
    }
    
    public bool DisableMenuCameraNoise {
        get => BetterBeatSaberConfig.Instance.DisableMenuCameraNoise;
        set => BetterBeatSaberConfig.Instance.DisableMenuCameraNoise = value;
    }
    
    public bool TransparentObstacles {
        get => BetterBeatSaberConfig.Instance.TransparentObstacles;
        set => BetterBeatSaberConfig.Instance.TransparentObstacles = value;
    }
    
    public bool HideLevelEnvironment {
        get => BetterBeatSaberConfig.Instance.HideLevelEnvironment;
        set => BetterBeatSaberConfig.Instance.HideLevelEnvironment = value;
    }
    
    public bool HideMenuEnvironment {
        get => BetterBeatSaberConfig.Instance.HideMenuEnvironment;
        set => BetterBeatSaberConfig.Instance.HideMenuEnvironment = value;
    }

}