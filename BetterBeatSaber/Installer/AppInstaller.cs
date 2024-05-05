using BetterBeatSaber.Extensions;
using BetterBeatSaber.Interops;

namespace BetterBeatSaber.Installer;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class AppInstaller : Zenject.Installer {

    public override void InstallBindings() {
        
        Container.BindInstance(BetterBeatSaber.Instance).AsSingle();
        
        Container.BindInterop<HitScoreVisualizer>();
        Container.BindInterop<SongCore>();
        Container.BindInterop<Tweaks55>();
        
    }
    
}