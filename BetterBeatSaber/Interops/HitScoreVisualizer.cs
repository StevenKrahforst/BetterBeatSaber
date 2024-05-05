using BetterBeatSaber.Interop;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using IPA.Loader;

namespace BetterBeatSaber.Interops;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class HitScoreVisualizer : Interop<HitScoreVisualizer> {

    protected override string Plugin => "HitScoreVisualizer";
    
    protected override void Init(PluginMetadata pluginMetadata) =>
        Logger.Warn("HitScoreVisualizer has been unpatched, it won't run until HitScore is disabled in Extras and after the game has been restarted");

    // ReSharper disable UnusedType.Global
    // ReSharper disable UnusedMember.Local
    // ReSharper disable InconsistentNaming
    
    [Mixin("HitScoreVisualizer", "HitScoreVisualizer.Installers.HsvMenuInstaller")]
    [Mixin("HitScoreVisualizer", "HitScoreVisualizer.Installers.HsvAppInstaller")]
    internal static class HitScoreVisualizerMixin {

        [MixinMethod(nameof(InstallBindings), MixinAt.Pre)]
        private static bool InstallBindings() => !BetterBeatSaberConfig.Instance.HitScoreEnable.CurrentValue;

    }

}