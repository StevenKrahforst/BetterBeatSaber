using BetterBeatSaber.Discord;

namespace BetterBeatSaber.Installer;

public sealed class AppInstaller : Zenject.Installer {

    public override void InstallBindings() {
        Container.BindInstance(BetterBeatSaber.Instance).AsSingle();
        Container.BindInterfacesAndSelfTo<DiscordManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<Manager.ColorManager>().AsSingle();
    }
    
}