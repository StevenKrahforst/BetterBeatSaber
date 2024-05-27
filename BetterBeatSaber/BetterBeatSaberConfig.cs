using System;
using System.Diagnostics;
using System.IO;

using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Enums;
using BetterBeatSaber.HudModifier;
using BetterBeatSaber.Utilities;

namespace BetterBeatSaber; 

// ReSharper disable ReplaceAutoPropertyWithComputedProperty

internal sealed class BetterBeatSaberConfig(string name) : Config.Config<BetterBeatSaberConfig>(name) {

    #region Handlers

    protected override void OnLoad(bool firstLoad) {

        if (!firstLoad)
            return;

        // ReSharper disable once InvertIf
        if (SignalRGBIntegration) {
            var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WhirlwindFX", "Effects", "bbs.html");
            if (!File.Exists(path)) {
                try {
                    using var stream = File.OpenWrite(path);
                    typeof(BetterBeatSaberConfig).Assembly.GetManifestResourceStream("BetterBeatSaber.Resources.bbs.html")!.CopyTo(stream);
                } catch (Exception) {
                    BetterBeatSaber.Instance.Logger.Warn("Failed to extract SignalRGB Integration Effect");
                }
            } else {
                Process.Start(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "VortxEngine", "SignalRgbLauncher.exe"), "--url=effect/apply/Better%20Beat%20Saber?-silentlaunch-");
            }
        }
        
    }

    #endregion

    #region Config

    public ObservableValue<float> ColorUpdateDurationTime { get; set; } = new(5f);

    #region Integrations

    #region RGB

    public bool SignalRGBIntegration { get; set; } = false;

    public ObservableValue<float> RGBIntegrationUpdateInterval { get; } = new(.02f);

    #endregion
    
    #endregion
    
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
    
    public bool DisableAprilFoolsAndEarthDayStuff { get; set; } = true;
    // TODO: Make Observable
    public bool HideEditorButton { get; set; } = true;
    // TODO: Make Observable
    public bool HidePromotionButton { get; set; } = true;
    
    public ObservableBoolean HideLevelEnvironment { get; set; } = new(true, "CustomFloorPlugin");
    public string[] IgnoredLevelGameObjects { get; set; } = [
        PlayersPlaceColorizer.GameObjectName,
        "DustPS"
    ];
    
    public ObservableBoolean HideMenuEnvironment { get; set; } = new(true, "CustomFloorPlugin");
    public string[] MenuGameObjects { get; set; } = [
        "MenuFogRing",
        "BackgroundGradient",
        "BasicMenuGround",
        "Notes",
        "PileOfNotes"
    ];
    
    public ObservableValue<bool> FakeChroma { get; set; } = new(false);
    
    #endregion

    #region Extras

    #region Hit Score

    public ObservableValue<bool> HitScoreEnable { get; set; } = new(true);
    public bool HitScoreBloom { get; set; } = true;
    public float HitScoreScale { get; set; } = 1.05f;
    
    public HitScoreMode HitScoreMode { get; set; } = HitScoreMode.Accuracy;

    #endregion
    
    #endregion

    #region HUD

    public bool RemoveHudBackground { get; set; } = true;
    
    public HudModifierOptions ComboHudModifier { get; set; } = new();
    public EnergyHudModifier.Options EnergyHudModifier { get; set; } = new();
    public HudModifierOptions MultiplierHudModifier { get; set; } = new();
    public HudModifierOptions ProgressHudModifier { get; set; } = new();
    public ScoreHudModifier.Options ScoreHudModifier { get; set; } = new();

    #endregion
    
    #endregion
    
    public class HudModifierOptions {

        public bool Enable { get; set; } = false;
        public bool Glow { get; set; } = true;

    }
    
}