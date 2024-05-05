using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using BeatSaberMarkupLanguage.Attributes;

using BetterBeatSaber.Enums;

using JetBrains.Annotations;

namespace BetterBeatSaber.UI.Main;

// ReSharper disable UnusedMember.Global

internal partial class MainView {

    [UIValue(nameof(HitScoreModes)), UsedImplicitly]
    protected List<object> HitScoreModes => new[] {
        Enums.HitScoreMode.Accuracy,
        Enums.HitScoreMode.Total,
        Enums.HitScoreMode.AccuracyWithTimeDependency,
        Enums.HitScoreMode.TotalWithTimeDependency
    }.Select(mode => mode.ToString()).Cast<object>().ToList();
    
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
    
    public string HitScoreMode {
        get => BetterBeatSaberConfig.Instance.HitScoreMode.ToString();
        set => BetterBeatSaberConfig.Instance.HitScoreMode = (HitScoreMode) Enum.Parse(typeof(HitScoreMode), value);
    }

    #endregion

    #region Integrations

    public bool SignalRGBIntegration {
        get => BetterBeatSaberConfig.Instance.SignalRGBIntegration;
        set => BetterBeatSaberConfig.Instance.SignalRGBIntegration = value;
    }

    [UIAction(nameof(OpenPhilipsHueGetStartedPage))]
    public void OpenPhilipsHueGetStartedPage() =>
        Process.Start("https://developers.meethue.com/develop/get-started-2/");

    #endregion

}