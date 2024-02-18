using System.Collections.Generic;

using BetterBeatSaber.Colorizer;
using BetterBeatSaber.HudModifier;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber; 

// CustomFloorPlugin check to disable hide level/menu env

public sealed class BetterBeatSaberConfig : Config.Config<BetterBeatSaberConfig> {

    // ReSharper disable once ConvertToPrimaryConstructor
    public BetterBeatSaberConfig(string name) : base(name) { }
    
    public ObservableValue<float> ColorUpdateDurationTime { get; set; } = 5f;
    
    #region Game Colorization

    public ObservableValue<bool> ColorizeDust { get; set; } = true;
    public ObservableValue<bool> ColorizeFeet { get; set; } = true;
    public ObservableValue<bool> ColorizePlayersPlace { get; set; } = true;
    public ObservableValue<bool> ColorizeBurnMarks { get; set; } = true;
    public ObservableValue<bool> ColorizeObstacles { get; set; } = true;
    public ObservableValue<bool> ColorizeCutParticles { get; set; } = true;
    public ObservableValue<bool> ColorizeMenuPillars { get; set; } = true;
    public ObservableValue<bool> ColorizeMenuSign { get; set; } = true;
    public ObservableValue<bool> ColorizeArcs { get; set; } = true;
    public ObservableValue<bool> ColorizeNoteDebris { get; set; } = true;
    public ObservableValue<bool> ColorizeReeSabers { get; set; } = true;
    
    public Outline.Config NoteOutlines { get; set; } = new();
    public Outline.Config BombOutlines { get; set; } = new();
    
    public bool ColorizeCustomNoteOutlines { get; set; } = true;
    
    #endregion

    #region UI Colorization

    public ObservableValue<bool> ColorizeButtons { get; set; } = true;
    // TODO: Update
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
    public bool DisableAprilFoolsAndEarthDayStuff { get; set; } = true;
    // TODO: Make Observable
    public bool HideEditorButton { get; set; } = true;
    // TODO: Make Observable
    public bool HidePromotionButton { get; set; } = true;
    
    public ObservableValue<bool> HideLevelEnvironment { get; set; } = true;
    public List<string> IgnoredLevelGameObjects { get; } = [
        PlayersPlaceColorizer.GameObjectName,
        "DustPS"
    ];
    
    public ObservableValue<bool> HideMenuEnvironment { get; set; } = true;
    public List<string> MenuGameObjects { get; } = [
        "MenuFogRing",
        "BackgroundGradient",
        "BasicMenuGround",
        "Notes",
        "PileOfNotes"
    ];
    
    #endregion

    #region Hit Score

    public ObservableValue<bool> HitScoreEnable { get; set; } = true;
    public bool HitScoreBloom { get; set; } = true;
    public float HitScoreScale { get; set; } = 1.05f;
    public bool HitScoreTotalScore { get; set; }

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