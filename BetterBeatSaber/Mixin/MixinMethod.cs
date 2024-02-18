using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Mixin.Exceptions;

using HarmonyLib;

using IPA.Loader;

namespace BetterBeatSaber.Mixin;

public sealed class MixinMethod(
    MixinManager mixinManager,
    Mixin mixin,
    string originalMethodName,
    MethodInfo patchMethod,
    MixinAt at,
    int priority = MixinManager.DefaultPriority,
    IEnumerable<string>? conflictsWith = null,
    IEnumerable<string>? before = null,
    IEnumerable<string>? after = null
) : MixinObject(mixinManager) {

    public Mixin Mixin { get; } = mixin;
    
    public string OriginalMethodName { get; } = originalMethodName;
    public MethodInfo PatchMethod { get; } = patchMethod;
    public MixinAt At { get; } = at;
    
    public int Priority { get; } = priority;
    
    public IEnumerable<string>? ConflictsWith { get; } = conflictsWith;
    public IEnumerable<string>? Before { get; } = before;
    public IEnumerable<string>? After { get; } = after;

    public bool ShouldPatch { get; set; } = true;
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsPatched { get; private set; }

    internal MethodInfo GetOriginalMethod(int differentApproach = 0) {
        try {
            
            if(differentApproach == 0)
                return Mixin
                    .TypeResolver
                    .ResolveType()
                    .GetMethod(OriginalMethodName, MixinManager.OriginalMethodBindingFlags)!;

            return Mixin
                .TypeResolver
                .ResolveType()
                .GetMethods(MixinManager.OriginalMethodBindingFlags)
                .FirstOrDefault(method => method.Name == OriginalMethodName)!;

        } catch(NullReferenceException exception) {
            throw new MixinOriginalMethodNotFoundException(MixinError.OriginalMethodNotFound, this, exception);
        } catch (AmbiguousMatchException exception) {
            if(differentApproach == 1)
                throw new MixinOriginalMethodNotFoundException(MixinError.OriginalMethodAmbiguousMatch, this, exception);
            return GetOriginalMethod(differentApproach+1);
        } catch (Exception exception) {
            throw new MixinObjectException(MixinError.UnknownError, this, exception);
        }
    }
    
    internal override void Patch() {

        if (!ShouldPatch)
            return;
        
        var conflict = ConflictsWith?.FirstOrDefault(plugin => PluginManager.GetPluginFromId(plugin) != null);
        if(conflict != null)
            throw new MixinConflictException(this, conflict);

        switch (At) {
            case MixinAt.Pre:
                MixinManager.Harmony.Patch(GetOriginalMethod(), prefix: new HarmonyMethod(PatchMethod, Priority, Before?.ToArray(), After?.ToArray()));
                break;
            case MixinAt.Post:
                MixinManager.Harmony.Patch(GetOriginalMethod(), postfix: new HarmonyMethod(PatchMethod, Priority, Before?.ToArray(), After?.ToArray()));
                break;
            case MixinAt.Trans:
                MixinManager.Harmony.Patch(GetOriginalMethod(), transpiler: new HarmonyMethod(PatchMethod, Priority, Before?.ToArray(), After?.ToArray()));
                break;
            default:
                throw new MixinObjectException(MixinError.UnsupportedOperation, this);
        }
        
        IsPatched = true;
        
    }

    internal override void Unpatch() {
        MixinManager.Harmony.Unpatch(GetOriginalMethod(), PatchMethod);
        IsPatched = false;
    }
    
}