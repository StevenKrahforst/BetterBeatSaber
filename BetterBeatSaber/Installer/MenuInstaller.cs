using BetterBeatSaber.Colorizer;
using BetterBeatSaber.Utilities;

namespace BetterBeatSaber.Installer; 

public sealed class MenuInstaller : Zenject.Installer {

    public override void InstallBindings() {
        
        if (BetterBeatSaberConfig.Instance.ColorizeDust)
            Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        
        if (BetterBeatSaberConfig.Instance.ColorizeFeet)
            Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();

        // TODO: Update
        if (!BetterBeatSaberConfig.Instance.HideMenuEnvironment)
            return;
        
        EnvironmentHider.LoadMenuGameObjects();
        EnvironmentHider.SetMenuEnvironment(!BetterBeatSaberConfig.Instance.HideMenuEnvironment);

    }

}