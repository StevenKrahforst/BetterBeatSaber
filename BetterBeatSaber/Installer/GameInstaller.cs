using BetterBeatSaber.Colorizer;
using BetterBeatSaber.HudModifier;
using BetterBeatSaber.Utilities;

namespace BetterBeatSaber.Installer; 

public sealed class GameInstaller : Zenject.Installer {

    public override void InstallBindings() {

        if (BetterBeatSaberConfig.Instance.ColorizeDust)
            Container.BindInterfacesAndSelfTo<DustColorizer>().AsSingle();
        
        if (BetterBeatSaberConfig.Instance.ColorizeFeet)
            Container.BindInterfacesAndSelfTo<FeetColorizer>().AsSingle();
        
        if (BetterBeatSaberConfig.Instance.ColorizePlayersPlace)
            Container.BindInterfacesAndSelfTo<PlayersPlaceColorizer>().AsSingle();
        
        // TODO: Maybe check if they are enabled?!
        
        BindHudModifier<ComboHudModifier>();
        BindHudModifier<EnergyHudModifier>();
        BindHudModifier<MultiplierHudModifier>();
        BindHudModifier<ProgressHudModifier>();
        BindHudModifier<RemoveBackgroundHudModifier>();
        BindHudModifier<ScoreHudModifier>();
        
        if (BetterBeatSaberConfig.Instance.HideLevelEnvironment) {
            EnvironmentHider.HideEnvironment();
        }

    }
    
    private void BindHudModifier<T>() where T : HudModifier.HudModifier =>
        Container.BindInterfacesAndSelfTo<T>().AsSingle();        
        
}