using BetterBeatSaber.Bindings;
using BetterBeatSaber.Colorizer;

namespace BetterBeatSaber.Installer; 

internal sealed class MenuInstaller : Zenject.Installer {

    public override void InstallBindings() {
        
        Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();
        
        if(BetterBeatSaberConfig.Instance.ColorizeMenuSign)
            Container.BindInterfacesAndSelfTo<MenuSignColorizer>().AsSingle();

        Container.BindInterfacesAndSelfTo<MenuEnvironmentHider>().AsSingle();

    }

}