using BetterBeatSaber.Mixin;
using BetterBeatSaber.Online.Installers;

using IPA.Logging;

using SiraUtil.Zenject;

namespace BetterBeatSaber.Online;

// COOL:
// https://github.com/Cysharp/UniTask

// ReSharper disable once UnusedType.Global
// ReSharper disable UnusedMember.Global

public sealed class BetterBeatSaberOnline(
    Logger logger,
    Zenjector zenjector,
    MixinManager mixinManager
) {
    
    internal Logger Logger { get; } = logger.GetChildLogger("Online");
    internal Zenjector Zenjector { get; } = zenjector;
    internal MixinManager MixinManager { get; } = mixinManager;

    /// <summary>
    /// Invoked by <see cref="OnlineLoader.LoadAsync"/>.
    /// </summary>
    internal void Init() {
        
        Logger.Info("INIT");
        
        Zenjector.Install<AppInstaller>(Location.App);
        Zenjector.Install<GameInstaller>(Location.GameCore);
        
    }

    /// <summary>
    /// Invoked by <see cref="OnlineLoader.Start"/>.
    /// </summary>
    internal void Start() {
        
        Logger.Info("START");
        
        //MixinManager.AddMixins();
        
    }

    /// <summary>
    /// Invoked by <see cref="OnlineLoader.Exit"/>.
    /// </summary>
    internal void Exit() {
        Logger.Info("EXIT");
    }

}