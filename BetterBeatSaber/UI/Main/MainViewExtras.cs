namespace BetterBeatSaber.UI.Main;

public partial class MainView {

    #region Hit Score

    public bool HitScoreEnable {
        get => BetterBeatSaberConfig.Instance.HitScoreEnable.CurrentValue;
        set => BetterBeatSaberConfig.Instance.HitScoreEnable.SetValue(value);
    }
    
    public bool HitScoreGlow {
        get => BetterBeatSaberConfig.Instance.HitScoreBloom;
        set => BetterBeatSaberConfig.Instance.HitScoreBloom = value;
    }
    
    public float HitScoreScale {
        get => BetterBeatSaberConfig.Instance.HitScoreScale * 100;
        set => BetterBeatSaberConfig.Instance.HitScoreScale = value / 100;
    }
    
    public bool HitScoreTotalScore {
        get => BetterBeatSaberConfig.Instance.HitScoreTotalScore;
        set => BetterBeatSaberConfig.Instance.HitScoreTotalScore = value;
    }

    #endregion
    
}