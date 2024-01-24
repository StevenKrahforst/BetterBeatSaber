namespace BetterBeatSaber.UI.Main;

public partial class MainView {

    #region Combo

    public bool ComboHudModifierEnable {
        get => BetterBeatSaberConfig.Instance.ComboHudModifier.Enable;
        set => BetterBeatSaberConfig.Instance.ComboHudModifier.Enable = value;
    }
    
    public bool ComboHudModifierGlow {
        get => BetterBeatSaberConfig.Instance.ComboHudModifier.Glow;
        set => BetterBeatSaberConfig.Instance.ComboHudModifier.Glow = value;
    }

    #endregion

    #region Energy

    public bool EnergyHudModifierEnable {
        get => BetterBeatSaberConfig.Instance.EnergyHudModifier.Enable;
        set => BetterBeatSaberConfig.Instance.EnergyHudModifier.Enable = value;
    }
    
    public bool EnergyHudModifierGlow {
        get => BetterBeatSaberConfig.Instance.EnergyHudModifier.Glow;
        set => BetterBeatSaberConfig.Instance.EnergyHudModifier.Glow = value;
    }
    
    public bool EnergyHudModifierShakeOnComboBreak {
        get => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeOnComboBreak;
        set => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeOnComboBreak = value;
    }
    
    public float EnergyHudModifierShakeStartIntensity {
        get => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeStartIntensity;
        set => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeStartIntensity = value;
    }
    
    public float EnergyHudModifierShakeIntensity {
        get => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeIntensity;
        set => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeIntensity = value;
    }
    
    public float EnergyHudModifierShakeStartDuration {
        get => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeStartDuration;
        set => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeStartDuration = value;
    }
    
    public float EnergyHudModifierShakeDuration {
        get => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeDuration;
        set => BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeDuration = value;
    }
    
    #endregion

    #region Multiplier

    public bool MultiplierHudModifierEnable {
        get => BetterBeatSaberConfig.Instance.MultiplierHudModifier.Enable;
        set => BetterBeatSaberConfig.Instance.MultiplierHudModifier.Enable = value;
    }
    
    public bool MultiplierHudModifierGlow {
        get => BetterBeatSaberConfig.Instance.MultiplierHudModifier.Glow;
        set => BetterBeatSaberConfig.Instance.MultiplierHudModifier.Glow = value;
    }

    #endregion

    #region Progress

    public bool ProgressHudModifierEnable {
        get => BetterBeatSaberConfig.Instance.ProgressHudModifier.Enable;
        set => BetterBeatSaberConfig.Instance.ProgressHudModifier.Enable = value;
    }
    
    public bool ProgressHudModifierGlow {
        get => BetterBeatSaberConfig.Instance.ProgressHudModifier.Glow;
        set => BetterBeatSaberConfig.Instance.ProgressHudModifier.Glow = value;
    }

    #endregion
    
    // TODO: Update
    #region Score

    public bool ScoreHudModifierEnable {
        get => BetterBeatSaberConfig.Instance.ScoreHudModifier.Enable;
        set => BetterBeatSaberConfig.Instance.ScoreHudModifier.Enable = value;
    }
    
    public bool ScoreHudModifierGlow {
        get => BetterBeatSaberConfig.Instance.ScoreHudModifier.Glow;
        set => BetterBeatSaberConfig.Instance.ScoreHudModifier.Glow = value;
    }

    #endregion
    
}