using System;

using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Mixin.Exceptions;

using IPA.Loader;

namespace BetterBeatSaber.Mixin.TypeResolvers;

public sealed class PluginResolver(string plugin, string typeName) : TypeResolver(typeName) {

    public string Plugin { get; } = plugin;

    public override Type ResolveType() {

        if (PluginManager.GetPluginFromId(Plugin) == null)
            throw new MixinNotFoundException(MixinError.PluginNotFound, $"Plugin {Plugin} not installed or found");
        
        var type = PluginManager
            .GetPluginFromId(Plugin)?
            .Assembly
            .GetType(TypeName);
        
        if(type == null)
            throw new MixinNotFoundException(MixinError.TypeNotFound, $"Type {TypeName} not found in Plugin {Plugin}");

        return type;

    }

}