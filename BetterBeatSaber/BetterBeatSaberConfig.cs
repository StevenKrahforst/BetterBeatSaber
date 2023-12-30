using BetterBeatSaber.HudModifier;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber; 

public sealed class BetterBeatSaberConfig : Config.Config<BetterBeatSaberConfig> {

    // ReSharper disable once ConvertToPrimaryConstructor
    public BetterBeatSaberConfig(string name) : base(name) { }
    
    #region Game Colorization

    public bool ColorizeDust { get; set; } = true;
    public bool ColorizeFeet { get; set; } = true;
    public bool ColorizePlayersPlace { get; set; } = true;
    public bool ColorizeBurnMarks { get; set; } = true;
    public bool ColorizeObstacles { get; set; } = true;
    public bool ColorizeCutParticles { get; set; } = true;
    public bool ColorizeNoteDebris { get; set; } = true;
    
    public Outline.OutlineConfig NoteOutlines { get; set; } = new();
    public Outline.OutlineConfig BombOutlines { get; set; } = new();
    
    public bool ColorizeCustomNoteOutlines { get; set; } = true;
    
    #endregion

    #region UI Colorization

    public bool ColorizeButtons { get; set; } = true;
    public bool ColorizeMenuButtons { get; set; } = true;
    
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public bool ColorizePBOT { get; set; } = true;
    // ReSharper disable once InconsistentNaming
    public bool ColorizeFCPercentage { get; set; } = true;
    
    #endregion
    
    #region Tweaks

    public float NoteSize { get; set; } = 1.2f;
    public float BombSize { get; set; } = 1.2f;
    
    public bool HideMissTexts { get; set; } = true;
    public bool DisableCutParticles { get; set; } = true;
    public bool DisableBombExplosionEffect { get; set; } = true;
    public bool DisableAprilFoolsAndEarthDayStuff { get; set; } = true;
    public bool DisableComboBreakEffect { get; set; } = true;
    public bool HideEditorButton { get; set; } = true;
    public bool HidePromotionButton { get; set; } = true;
    public bool DisableMenuCameraNoise { get; set; } = true;
    public bool TransparentObstacles { get; set; } = true;
    
    public bool HideLevelEnvironment { get; set; } = true;
    public bool HideMenuEnvironment { get; set; } = true;

    #endregion

    #region Hit Score

    public float HitScoreScale { get; set; } = 1.05f;
    public bool HitScoreGlow { get; set; } = true;

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

}