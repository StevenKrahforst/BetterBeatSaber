using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BetterBeatSaber.Mixin.Enums;

using HarmonyLib;

using IPA.Loader;

namespace BetterBeatSaber.Mixin;

internal sealed class MixinMethod(
    MixinManager mixinManager,
    Mixin mixin,
    string originalMethodName,
    MethodInfo patchMethod,
    MixinAt at,
    IEnumerable<string> conflictsWith
) : MixinObject(mixinManager) {

    public Mixin Mixin { get; } = mixin;
    
    public MethodInfo PatchMethod { get; } = patchMethod;
    public MixinAt At { get; } = at;
    public IEnumerable<string> ConflictsWith { get; } = conflictsWith;

    public bool IsPatched { get; set; }
    public bool ShouldPatch { get; set; } = true;

    public MethodInfo OriginalMethod {
        get {
            try {
                return Mixin
                    .TypeResolver
                    .ResolveType()
                    .GetMethod(originalMethodName, MixinManager.OriginalMethodBindingFlags)!;
            } catch(MixinException exception) {
                throw;
            } catch(NullReferenceException exception) {
                throw new MixinException(MixinError.MissingOriginalMethod, $"Mixin {Mixin.Type.FullName} is missing original method {originalMethodName}", exception);
            } catch (AmbiguousMatchException _) {
                try {
                    return Mixin
                        .TypeResolver
                        .ResolveType()
                        .GetMethods(MixinManager.OriginalMethodBindingFlags)
                        .FirstOrDefault(method => method.Name == originalMethodName)!;
                } catch (Exception exception) {
                    throw new MixinException(MixinError.AmbiguousMatchForOriginalMethod, $"Mixin {Mixin.Type.FullName} has ambiguous match for original method {originalMethodName} and while trying to take an alternative route, an error occurred", exception);
                }
            } catch (Exception exception) {
                throw new MixinException(MixinError.UnknownError, exception);
            }
        }
    }
    
    internal override void Patch() {

        if (!ShouldPatch)
            throw new MixinException(MixinError.ShouldNotPatch, $"Mixin {Mixin.Type.FullName} should not be patched");
        
        if(ConflictsWith.Any(plugin => PluginManager.GetPluginFromId(plugin) != null))
            throw new MixinException(MixinError.ConflictsWithPlugin, $"Mixin {Mixin.Type.FullName} conflicts with another Plugin");

        switch (At) {
            case MixinAt.Pre:
                MixinManager.Harmony.Patch(OriginalMethod, prefix: new HarmonyMethod(PatchMethod));
                break;
            case MixinAt.Post:
                MixinManager.Harmony.Patch(OriginalMethod, postfix: new HarmonyMethod(PatchMethod));
                break;
            case MixinAt.Trans:
                MixinManager.Harmony.Patch(OriginalMethod, transpiler: new HarmonyMethod(PatchMethod));
                break;
            default:
                throw new NotSupportedException();
        }
        
        IsPatched = true;
        
    }

    internal override void Unpatch() {
        
        MixinManager.Harmony.Unpatch(OriginalMethod, PatchMethod);
        
        IsPatched = false;
        
    }
    
}