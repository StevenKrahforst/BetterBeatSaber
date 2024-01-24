using BetterBeatSaber.Enums;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.UI.Main;

public partial class MainView {

    private GameObject? _bomb;
    private Outline? _bombOutline;

    private void CreateBomb() {
        
        if (_bombTemplate == null)
            return;
        
        _bombTemplate = Instantiate(_bombTemplate);
        
    }

    private void DestroyBomb() {
        Destroy(_bombOutline);
        Destroy(_bomb);
    }

    #region Config

    public float BombSize {
        get => BetterBeatSaberConfig.Instance.BombSize * 100;
        set => BetterBeatSaberConfig.Instance.BombSize = value / 100;
    }

    public bool BombColorize {
        get => BetterBeatSaberConfig.Instance.Bombs.Colorize;
        set => BetterBeatSaberConfig.Instance.Bombs.Colorize = value;
    }
    
    public bool BombRGB {
        get => BetterBeatSaberConfig.Instance.Bombs.RGB;
        set => BetterBeatSaberConfig.Instance.Bombs.RGB = value;
    }
    
    public Color BombFirstColor {
        get => BetterBeatSaberConfig.Instance.Bombs.FirstColor ?? Color.black;
        set => BetterBeatSaberConfig.Instance.Bombs.FirstColor = value;
    }
    
    public Color BombSecondColor {
        get => BetterBeatSaberConfig.Instance.Bombs.SecondColor ?? Color.black;
        set => BetterBeatSaberConfig.Instance.Bombs.SecondColor = value;
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
    
    public Visibility BombOutlinesVisibility {
        get => BetterBeatSaberConfig.Instance.BombOutlines.Visibility;
        set => BetterBeatSaberConfig.Instance.BombOutlines.Visibility = value;
    }

    #endregion

    #endregion
    
}