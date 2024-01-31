using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Mixin.TypeResolvers;

using IPA.Loader;

namespace BetterBeatSaber.Mixin;

internal sealed class Mixin(
    MixinManager mixinManager,
    Type type,
    TypeResolver typeResolver,
    IEnumerable<string> conflictsWith
) : MixinObject(mixinManager) {

    public Type Type { get; } = type;
    public TypeResolver TypeResolver { get; } = typeResolver;
    public IEnumerable<string> ConflictsWith { get; } = conflictsWith;

    public IEnumerable<MixinMethod> Methods { get; internal set; } = null!;
    public IEnumerable<MethodInfo> ActionHandlers { get; internal set; } = null!;

    internal void RunActionHandlers(MixinAction action) {
        foreach (var actionHandler in ActionHandlers)
            actionHandler.Invoke(null, actionHandler.GetParameters().Length == 1 ? [ action ] : []);
    }

    internal override void Patch() {
        
        if(ConflictsWith.Any(plugin => PluginManager.GetPluginFromId(plugin) != null))
            throw new MixinException(MixinError.ConflictsWithPlugin, $"Mixin {Type.FullName} conflicts with another Plugin");
        
        foreach (var mixinMethod in Methods)
            mixinMethod.Patch();

        RunActionHandlers(MixinAction.Patch);

    }
    
    internal override void Unpatch() {
        
        foreach (var mixinMethod in Methods)
            mixinMethod.Unpatch();
        
        RunActionHandlers(MixinAction.Unpatch);
        
    }
    
    protected override void OnToggleChanged(bool value) {
        RunActionHandlers(MixinAction.Toggle);
        base.OnToggleChanged(value);
    }

}