using System;

using BetterBeatSaber.Enums;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.UI.Main;

public partial class MainView {

    /*private GameObject? _bomb;
    private Outline? _bombOutline;

    private void CreateBomb() {
        
        if (_bombTemplate == null)
            return;
        
        _bombTemplate = Instantiate(_bombTemplate);
        
    }

    private void DestroyBomb() {
        Destroy(_bombOutline);
        Destroy(_bomb);
    }*/

    #region Config

    public float BombSize {
        get => BetterBeatSaberConfig.Instance.BombSize * 100;
        set => BetterBeatSaberConfig.Instance.BombSize.SetValue(value / 100);
    }

    public bool BombColorize {
        get => BetterBeatSaberConfig.Instance.ColorizeBombs;
        set => BetterBeatSaberConfig.Instance.ColorizeBombs = value;
    }
    
    #region Outlines

    public bool BombOutlinesEnable {
        get => BetterBeatSaberConfig.Instance.BombOutlines.Enable;
        set => BetterBeatSaberConfig.Instance.BombOutlines.Enable = value;
    }
    
    public bool BombOutlinesGlow {
        get => BetterBeatSaberConfig.Instance.BombOutlines.Bloom;
        set => BetterBeatSaberConfig.Instance.BombOutlines.Bloom = value;
    }
    
    public float BombOutlinesWidth {
        get => BetterBeatSaberConfig.Instance.BombOutlines.Width;
        set => BetterBeatSaberConfig.Instance.BombOutlines.Width = value;
    }
    
    public string BombOutlinesVisibility {
        get => BetterBeatSaberConfig.Instance.BombOutlines.Visibility.ToString();
        set {
            if (Enum.TryParse<Visibility>(value, out var visibility))
                BetterBeatSaberConfig.Instance.BombOutlines.Visibility = visibility;
        }
    }

    #endregion

    #endregion
    
}