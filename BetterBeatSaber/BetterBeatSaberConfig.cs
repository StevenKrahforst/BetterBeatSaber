using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Enums;
using BetterBeatSaber.HudModifier;
using BetterBeatSaber.Utilities;

using IPA.Config.Stores.Converters;

using Newtonsoft.Json;

namespace BetterBeatSaber; 

// CustomFloorPlugin check to disable hide level/menu env

internal sealed class BetterBeatSaberConfig : Config.Config<BetterBeatSaberConfig> {

    // ReSharper disable once ConvertToPrimaryConstructor
    public BetterBeatSaberConfig(string name) : base(name) { }
    
    public ObservableValue<float> ColorUpdateDurationTime { get; set; } = new(5f);
    
    #region Game Colorization

    public ObservableValue<bool> ColorizeDust { get; set; } = new(true);
    public ObservableValue<bool> ColorizeFeet { get; set; } = new(true);
    public ObservableValue<bool> ColorizePlayersPlace { get; set; } = new(true);
    public ObservableValue<bool> ColorizeBurnMarks { get; set; } = new(true);
    public ObservableValue<bool> ColorizeObstacles { get; set; } = new(true);
    public ObservableValue<bool> ColorizeCutParticles { get; set; } = new(true);
    public ObservableValue<bool> ColorizeMenuPillars { get; set; } = new(true);
    public ObservableValue<bool> ColorizeMenuSign { get; set; } = new(true);
    public ObservableValue<bool> ColorizeArcs { get; set; } = new(true);
    public ObservableValue<bool> ColorizeNoteDebris { get; set; } = new(true);
    public ObservableValue<bool> ColorizeReeSabers { get; set; } = new(true);
    
    public Outline.Config NoteOutlines { get; set; } = new();
    public Outline.Config BombOutlines { get; set; } = new();
    
    public bool ColorizeCustomNoteOutlines { get; set; } = true;
    
    #endregion

    #region UI Colorization

    public ObservableValue<bool> ColorizeButtons { get; set; } = new(true);
    // TODO: Update
    public bool ColorizeMenuButtons { get; set; } = true;
    
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once IdentifierTypo
    public bool ColorizePBOT { get; set; } = true;
    // ReSharper disable once InconsistentNaming
    public bool ColorizeFCPercentage { get; set; } = true;
    
    public bool ColorizeFPSCounter { get; set; } = true;
    public bool ColorizeBombs { get; set; } = true;
    
    #endregion
    
    #region Tweaks

    public ObservableValue<float> NoteSize { get; set; } = new(1.2f);
    public ObservableValue<float> BombSize { get; set; } = new(1.2f);
    
    public bool HideMissTexts { get; set; } = true;
    public bool DisableAprilFoolsAndEarthDayStuff { get; set; } = true;
    // TODO: Make Observable
    public bool HideEditorButton { get; set; } = true;
    // TODO: Make Observable
    public bool HidePromotionButton { get; set; } = true;
    
    public ObservableValue<bool> HideLevelEnvironment { get; set; } = new(true);
    public string[] IgnoredLevelGameObjects { get; set; } = [
        PlayersPlaceColorizer.GameObjectName,
        "DustPS"
    ];
    
    public ObservableValue<bool> HideMenuEnvironment { get; set; } = new(true);
    public string[] MenuGameObjects { get; set; } = [
        "MenuFogRing",
        "BackgroundGradient",
        "BasicMenuGround",
        "Notes",
        "PileOfNotes"
    ];
    
    public ObservableValue<bool> FakeChroma { get; set; } = new(false);
    
    #endregion

    #region Hit Score

    public ObservableValue<bool> HitScoreEnable { get; set; } = new(true);
    public bool HitScoreBloom { get; set; } = true;
    public float HitScoreScale { get; set; } = 1.05f;
    
    [JsonConverter(typeof(NumericEnumConverter<HitScoreMode>))]
    public HitScoreMode HitScoreMode { get; set; } = HitScoreMode.Accuracy;

    #endregion
    
    public HudModifier.HudModifier.BaseOptions ComboHudModifier { get; set; } = new();
    public EnergyHudModifier.Options EnergyHudModifier { get; set; } = new();
    public HudModifier.HudModifier.BaseOptions MultiplierHudModifier { get; set; } = new();
    public HudModifier.HudModifier.BaseOptions ProgressHudModifier { get; set; } = new();
    public ScoreHudModifier.Options ScoreHudModifier { get; set; } = new();

}