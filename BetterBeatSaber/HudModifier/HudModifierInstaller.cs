namespace BetterBeatSaber.HudModifier;

public sealed class HudModifierInstaller : Zenject.Installer {

    public override void InstallBindings() {
        
        if(BetterBeatSaberConfig.Instance.ComboHudModifier.Enable)
            BindHudModifier<ComboHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.EnergyHudModifier.Enable)
            BindHudModifier<EnergyHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.MultiplierHudModifier.Enable)
            BindHudModifier<MultiplierHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.ProgressHudModifier.Enable)
            BindHudModifier<ProgressHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.ScoreHudModifier.Enable)
            BindHudModifier<ScoreHudModifier>();
        
        if(BetterBeatSaberConfig.Instance.RemoveHudBackground)
            BindHudModifier<RemoveBackgroundHudModifier>();
        
    }

    private void BindHudModifier<T>() where T : IHudModifier =>
        Container.BindInterfacesAndSelfTo<T>().AsSingle();
    
}