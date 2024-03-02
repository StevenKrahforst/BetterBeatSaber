using System;

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

    internal void Init() {
        
        Zenjector.Install<AppInstaller>(Location.App);
        Zenjector.Install<GameInstaller>(Location.GameCore);
        
        MixinManager.AddMixins();
        
        Console.WriteLine("HEYYYY");
        
    }

    internal void Exit() {
    }

}