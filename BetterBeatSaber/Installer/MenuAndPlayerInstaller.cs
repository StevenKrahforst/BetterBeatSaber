using BetterBeatSaber.Providers;

namespace BetterBeatSaber.Installer;

internal sealed class MenuAndPlayerInstaller : Zenject.Installer {

    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<MaterialProvider>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BetterBloomFontProvider>().AsSingle().NonLazy();
    }

}