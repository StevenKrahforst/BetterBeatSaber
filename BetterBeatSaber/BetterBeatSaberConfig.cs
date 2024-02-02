using System.ComponentModel;

using BetterBeatSaber.Config.Attributes;
using BetterBeatSaber.HudModifier;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber; 

public sealed class BetterBeatSaberConfig : Config.Config<BetterBeatSaberConfig>, INotifyPropertyChanged {

    // ReSharper disable once ConvertToPrimaryConstructor
    public BetterBeatSaberConfig(string name) : base(name) { }

    public bool SendTelemetry { get; set; } = true;
    
    [ConflictsWith("bsrpc")]
    public ObservableValue<bool> DiscordRichPresence { get; set; } = true;
    
    public ObservableValue<float> ColorUpdateDurationTime { get; set; } = 5f;
    
    #region Game Colorization

    public ObservableValue<bool> ColorizeDust { get; set; } = true;
    public ObservableValue<bool> ColorizeFeet { get; set; } = true;
    public ObservableValue<bool> ColorizePlayersPlace { get; set; } = true;
    public bool ColorizeBurnMarks { get; set; } = true;
    public bool ColorizeObstacles { get; set; } = true;
    public bool ColorizeCutParticles { get; set; } = true;
    public bool ColorizeArcs { get; set; } = true;
    public ObservableValue<bool> ColorizeNoteDebris { get; set; } = true;
    public ObservableValue<bool> ColorizeReeSabers { get; set; } = true;
    
    public Outline.Config NoteOutlines { get; set; } = new();
    public Outline.Config BombOutlines { get; set; } = new();
    
    public bool ColorizeCustomNoteOutlines { get; set; } = true;
    
    public ObservableValue<bool> ColorizeMenuPillars { get; set; } = true;
    public ObservableValue<bool> ColorizeMenuSign { get; set; } = true;
    
    #endregion

    #region UI Colorization

    public ObservableValue<bool> ColorizeButtons { get; set; } = true;
    public bool ColorizeMenuButtons { get; set; } = true;
    
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public bool ColorizePBOT { get; set; } = true;
    // ReSharper disable once InconsistentNaming
    public bool ColorizeFCPercentage { get; set; } = true;
    
    public bool ColorizeFPSCounter { get; set; } = true;
    
    #endregion
    
    #region Tweaks

    public ObservableValue<float> NoteSize { get; set; } = 1.2f;
    public ObservableValue<float> BombSize { get; set; } = 1.2f;
    
    public bool HideMissTexts { get; set; } = true;
    public ObservableValue<bool> DisableCutParticles { get; set; } = true;
    public ObservableValue<bool> DisableDust { get; set; } = true;
    public ObservableValue<bool> DisableBombExplosionEffect { get; set; } = true;
    public bool DisableAprilFoolsAndEarthDayStuff { get; set; } = true;
    public bool DisableComboBreakEffect { get; set; } = true;
    public bool HideEditorButton { get; set; } = true;
    public bool HidePromotionButton { get; set; } = true;
    public bool DisableMenuCameraNoise { get; set; } = true;
    public bool TransparentObstacles { get; set; } = true;
    
    [ConflictsWith("CustomFloorPlugin")]
    public ObservableValue<bool> HideLevelEnvironment { get; set; } = true;
    
    [ConflictsWith("CustomFloorPlugin")]
    public ObservableValue<bool> HideMenuEnvironment { get; set; } = true;

    #endregion

    #region Hit Score

    public ObservableValue<bool> HitScoreEnable { get; set; } = true;
    public bool HitScoreGlow { get; set; } = true;
    public float HitScoreScale { get; set; } = 1.05f;
    public bool HitScoreTotalScore { get; set; } = false;

    #endregion
    
    public BombConfig Bombs { get; set; } = new();
    
    public HudModifier.HudModifier.BaseOptions ComboHudModifier { get; set; } = new();
    public EnergyHudModifier.Options EnergyHudModifier { get; set; } = new();
    public HudModifier.HudModifier.BaseOptions MultiplierHudModifier { get; set; } = new();
    public HudModifier.HudModifier.BaseOptions ProgressHudModifier { get; set; } = new();
    public ScoreHudModifier.Options ScoreHudModifier { get; set; } = new();

    public class BombConfig {

        public bool Colorize { get; set; } = true;
        public bool RGB { get; set; } = true;
        public Color? FirstColor { get; set; }
        public Color? SecondColor { get; set; }

    }

    public override event PropertyChangedEventHandler? PropertyChanged;

}