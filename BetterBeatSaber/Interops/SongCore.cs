using System;
using System.Collections.Generic;
using System.Reflection;

using BetterBeatSaber.Interop;

using IPA.Loader;

namespace BetterBeatSaber.Interops;

internal sealed class SongCore : Interop<SongCore>, IDisposable {

    private static readonly List<string> ChromaCapabilities = [
        "Chroma",
        "Chroma Lighting Events"
    ];
    
    protected override string Plugin => "SongCore";
    
    private FieldInfo? _capabilitiesField;
    private MethodInfo? _registerCapabilityMethod;
    private MethodInfo? _unregisterCapabilityMethod;

    protected override void Init(PluginMetadata pluginMetadata) {
        var type = pluginMetadata.Assembly.GetType("SongCore.Collections");
        _capabilitiesField = type.GetField("_capabilities", BindingFlags.NonPublic | BindingFlags.Static);
        _registerCapabilityMethod = type.GetMethod("RegisterCapability", BindingFlags.Public | BindingFlags.Static);
        _unregisterCapabilityMethod = type.GetMethod("DeregisterizeCapability", BindingFlags.Public | BindingFlags.Static);
        SetChroma(BetterBeatSaberConfig.Instance.FakeChroma.CurrentValue);
        BetterBeatSaberConfig.Instance.FakeChroma.OnValueChanged += SetChroma;
    }

    protected override bool RunIf() {
        var installed = PluginManager.GetPluginFromId("Chroma") != null;
        if(installed)
            Logger.Info("Fake Chroma won't be enabled because Chroma is installed");
        return !installed;
    }

    private void SetChroma(bool state) {
        foreach (var chromaCapability in ChromaCapabilities)
            SetCapability(chromaCapability, state);
    }

    private bool HasCapability(string capability) =>
        _capabilitiesField?.GetValue(null) is List<string> list && list.Contains(capability);

    private void SetCapability(string capability, bool state) {
        switch (state) {
            case true when !HasCapability(capability):
                AddCapability(capability);
                break;
            case false when HasCapability(capability):
                RemoveCapability(capability);
                break;
        }
    }

    private void AddCapability(string capability) =>
        _registerCapabilityMethod?.Invoke(null, [ capability ]);
    
    private void RemoveCapability(string capability) =>
        _unregisterCapabilityMethod?.Invoke(null, [ capability ]);

    public void Dispose() {
        BetterBeatSaberConfig.Instance.FakeChroma.OnValueChanged -= SetChroma;
        _capabilitiesField = null;
        _registerCapabilityMethod = null;
        _unregisterCapabilityMethod = null;
    }
    
}