using System;
using System.Reflection;

using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Utilities;

using IPA.Loader;

namespace BetterBeatSaber.Interops;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class Tweaks55 : Interop.Interop<Tweaks55> {

    protected override string Plugin => "Tweaks55";

    public ObservableValue<bool> DisableCutParticles { get; } = new(false);
    public ObservableValue<bool> DisableGlobalParticles { get; } = new(false);
    
    private Type? _configType;
    private object? _configInstance;

    protected override void Init(PluginMetadata pluginMetadata) {
        
        _configType = pluginMetadata.Assembly.GetType("Tweaks55.Config");
        _configInstance = _configType.GetField("Instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null);

        UpdateValues();

    }

    private void UpdateValues() {
        
        var disableCutParticles = (bool?) _configType?.GetField("disableCutParticles")?.GetValue(_configInstance);
        if (disableCutParticles.HasValue && disableCutParticles.Value != DisableCutParticles.CurrentValue)
            DisableCutParticles.SetValue(disableCutParticles.Value);
        
        var disableGlobalParticles = (bool?) _configType?.GetField("disableGlobalParticles")?.GetValue(_configInstance);
        if (disableGlobalParticles.HasValue && disableGlobalParticles.Value != DisableGlobalParticles.CurrentValue)
            DisableGlobalParticles.SetValue(disableGlobalParticles.Value);
        
    }

    internal void OnValuesChanged() =>
        UpdateValues();
    
    // ReSharper disable UnusedType.Global
    // ReSharper disable UnusedMember.Local
    // ReSharper disable InconsistentNaming
    
    [Mixin("Tweaks55", "Tweaks55.Config")]
    internal static class Tweaks55Mixin {

        [MixinMethod(nameof(ApplyValues), MixinAt.Post)]
        private static void ApplyValues() =>
            Instance?.OnValuesChanged();

    }

}