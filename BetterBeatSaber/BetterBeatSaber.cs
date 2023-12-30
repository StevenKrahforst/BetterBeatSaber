using System.Collections;
using System.Reflection;

using BetterBeatSaber.Installer;
using BetterBeatSaber.Mixin;
using BetterBeatSaber.Utilities;

using IPA;
using IPA.Loader;

using JetBrains.Annotations;

using SiraUtil.Zenject;

using UnityEngine;

using Logger = IPA.Logging.Logger;

#if DEBUG
using UnityExplorer;
#endif

namespace BetterBeatSaber; 

// ReSharper disable UnusedMember.Global

[Plugin(RuntimeOptions.SingleStartInit)]
public sealed class BetterBeatSaber {

    public static BetterBeatSaber Instance { get; private set; } = null!;

    internal Logger Logger { get; private set; }
    
    internal Zenjector Zenjector { get; private set; }

    internal MixinManager MixinManager { get; private set; }
    
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
        
        MixinManager.Initialize();
        
        Zenjector.Install<MenuAndPlayerInstaller>(Location.Menu | Location.Player);
        
        Zenjector.Install<MenuInstaller>(Location.Menu);
        Zenjector.Install<GameInstaller>(Location.Player);
        
        Zenjector.Expose<ScoreMultiplierUIController>("Environment");
        Zenjector.Expose<SongProgressUIController>("Environment");
        Zenjector.Expose<ImmediateRankUIPanel>("Environment");
        
        Zenjector.Expose<ComboUIController>("Environment");
        Zenjector.Expose<GameEnergyUIPanel>("Environment");

        Utilities.SharedCoroutineStarter.Instance.StartCoroutine(LoadAssets());
        
        #if DEBUG
        if (Environment.GetCommandLineArgs().Contains("--explorer"))
            ExplorerStandalone.CreateInstance();
        #endif
        
    }

    [OnExit]
    public void Exit() {
        MixinManager.Exit();
    }

    private static IEnumerator LoadAssets() {
        
        var bundleRequest = AssetBundle.LoadFromStreamAsync(Assembly.GetExecutingAssembly().GetManifestResourceStream("BetterBeatSaber.Resources.resources"));
        yield return bundleRequest;
        
        var assetBundle = bundleRequest.assetBundle;
        if (assetBundle == null)
            yield break;

        var fillMatRequest = assetBundle.LoadAssetAsync<Material>("OutlineFill");
        yield return fillMatRequest;
        Outline.OutlineFillMaterialSource = (fillMatRequest.asset as Material)!;
        
        var maskMatRequest = assetBundle.LoadAssetAsync<Material>("OutlineMask");
        yield return maskMatRequest;
        Outline.OutlineMaskMaterialSource = (maskMatRequest.asset as Material)!;
        
        assetBundle.Unload(false);

    }
    
    private static MixinManager CreateMixinManager(object? previous, ParameterInfo param, PluginMetadata pluginMetadata) {
        return new MixinManager(pluginMetadata);
    }
    
}