﻿namespace BetterBeatSaber.UI.Main;

public partial class MainView {

    #region Hit Score

    public bool HitScoreEnable {
        get => BetterBeatSaberConfig.Instance.HitScoreEnable.CurrentValue;
        set => BetterBeatSaberConfig.Instance.HitScoreEnable.SetValue(value);
    }
    
    public bool HitScoreGlow {
        get => BetterBeatSaberConfig.Instance.HitScoreGlow;
        set => BetterBeatSaberConfig.Instance.HitScoreGlow = value;
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

    #region Other

    public bool SendTelemetry {
        get => BetterBeatSaberConfig.Instance.SendTelemetry;
        set => BetterBeatSaberConfig.Instance.SendTelemetry = value;
    }
    
    public bool DiscordRichPresence {
        get => BetterBeatSaberConfig.Instance.DiscordRichPresence.CurrentValue;
        set => BetterBeatSaberConfig.Instance.DiscordRichPresence.SetValue(value);
    }

    #endregion
    
}