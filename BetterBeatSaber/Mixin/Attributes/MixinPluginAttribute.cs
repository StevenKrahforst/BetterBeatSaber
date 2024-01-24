using System;

using IPA.Loader;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MixinPluginAttribute : MixinAttribute {

    public string PluginId { get; set; }
    
    public PluginMetadata? Plugin => PluginManager.GetPluginFromId(PluginId);

    // ReSharper disable once ConvertToPrimaryConstructor
    public MixinPluginAttribute(string pluginId, string typeName) : base(typeName) {
        PluginId = pluginId;
    }

    public override bool ShouldRun() =>
        Plugin != null;

    protected override Type? ResolveType(string typeName) =>
        Plugin != null ? Plugin.Assembly.GetType(typeName) : base.ResolveType(typeName);
    
}