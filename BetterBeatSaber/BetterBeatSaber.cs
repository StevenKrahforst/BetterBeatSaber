using System;
using System.Collections;
using System.Reflection;

using BeatSaberMarkupLanguage;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.HudModifier;
using BetterBeatSaber.Installer;
using BetterBeatSaber.Mixin;
using BetterBeatSaber.Providers;
using BetterBeatSaber.Utilities;

using IPA;
using IPA.Loader;

using JetBrains.Annotations;

using SiraUtil.Zenject;

using TMPro;

using UnityEngine;
using UnityEngine.TextCore.LowLevel;

using Logger = IPA.Logging.Logger;

#if DEBUG
//using UnityExplorer;
#endif

namespace BetterBeatSaber; 

// ReSharper disable UnusedMember.Global

[Plugin(RuntimeOptions.SingleStartInit), NoEnableDisable]
public sealed class BetterBeatSaber {

    public static BetterBeatSaber Instance { get; private set; } = null!;

    internal Logger Logger { get; }
    
    internal Zenjector Zenjector { get; }

    internal MixinManager MixinManager { get; }
    
    [Init]
    public BetterBeatSaber([UsedImplicitly] Logger logger, [UsedImplicitly] Zenjector zenjector) {

        Instance = this;

        Logger = logger;
        Zenjector = zenjector;
        
        // ReSharper disable once ObjectCreationAsStatement
        new BetterBeatSaberConfig("BetterBeatSaber");

        PluginInitInjector.AddInjector(typeof(BetterBeatSaber), (_, _, _) => this);
        PluginInitInjector.AddInjector(typeof(MixinManager), CreateMixinManager);
        
        MixinManager = new MixinManager("BetterBeatSaber", Assembly.GetExecutingAssembly());
        MixinManager.AddMixins();
        
    }

    [OnStart]
    public void Start() {
        
        MixinManager.Patch();
        
        Zenjector.Expose<ScoreMultiplierUIController>("Environment");
        Zenjector.Expose<SongProgressUIController>("Environment");
        Zenjector.Expose<ImmediateRankUIPanel>("Environment");
        Zenjector.Expose<ComboUIController>("Environment");
        Zenjector.Expose<GameEnergyUIPanel>("Environment");
        
        Zenjector.Install<AppInstaller>(Location.App);
        
        Zenjector.Install<MenuAndPlayerInstaller>(Location.Menu | Location.Player);
        
        Zenjector.Install<MenuInstaller>(Location.Menu);
        Zenjector.Install<GameInstaller>(Location.Player);
        Zenjector.Install<HudModifierInstaller>(Location.StandardPlayer | Location.CampaignPlayer);
        
        Utilities.SharedCoroutineStarter.Instance.StartCoroutine(LoadAssets());

        #if DEBUG
        //if (Environment.GetCommandLineArgs().Contains("--explorer"))
        //    ExplorerStandalone.CreateInstance();
        #endif
        
        UI.MainFlowCoordinator.Initialize();
        
    }

    [OnExit]
    public void Exit() {
        MixinManager.Unpatch();
    }

    // Move to class like "AssetLoader" or "AssetBundleLoader"
    private static IEnumerator LoadAssets() {
        
        var bundleRequest = AssetBundle.LoadFromStreamAsync(Assembly.GetExecutingAssembly().GetManifestResourceStream("BetterBeatSaber.Resources.resources"));
        yield return bundleRequest;
        
        var assetBundle = bundleRequest.assetBundle;
        if (assetBundle == null)
            yield break;

        var maskMaterialRequest = assetBundle.LoadAssetAsync<Material>("Outline Mask");
        yield return maskMaterialRequest;
        Outline.MaskMaterial = (maskMaterialRequest.asset as Material)!;
        
        var fillMaterialRequest = assetBundle.LoadAssetAsync<Material>("Outline Fill");
        yield return fillMaterialRequest;
        Outline.FillMaterial = (fillMaterialRequest.asset as Material)!;

        var bloomFontRequest = assetBundle.LoadAssetAsync<Font>("Teko-Medium Bloom");
        yield return bloomFontRequest;
        TextMeshProExtensions.BloomFont = TMP_FontAsset.CreateFontAsset(bloomFontRequest.asset as Font, 90, 5, GlyphRenderMode.SDFAA, 1024, 512, AtlasPopulationMode.Static);
        
        assetBundle.Unload(false);
        
    }
    
    private static MixinManager CreateMixinManager(object? _, ParameterInfo __, PluginMetadata pluginMetadata) =>
        new(pluginMetadata);

}