using BetterBeatSaber.Online.Manager;

namespace BetterBeatSaber.Online.Installers;

public sealed class AppInstaller : Zenject.Installer {

    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<NetworkManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<AuthManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<DiscordManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<FriendManager>().AsSingle();
    }

}