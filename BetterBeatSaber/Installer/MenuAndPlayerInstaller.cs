using BetterBeatSaber.Providers;

namespace BetterBeatSaber.Installer;

public sealed class MenuAndPlayerInstaller : Zenject.Installer {

    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<MaterialProvider>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BloomFontProvider>().AsSingle().NonLazy();
    }

}