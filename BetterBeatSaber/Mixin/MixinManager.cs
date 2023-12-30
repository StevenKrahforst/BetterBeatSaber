using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IPA.Loader;
using IPA.Logging;

namespace BetterBeatSaber.Mixin;

public sealed class MixinManager : IDisposable {

    private const BindingFlags OriginalMethodBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
    private const BindingFlags PatchedMethodBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
    
    public Assembly? Assembly { get; }
    public HarmonyLib.Harmony Harmony { get; }
    
    private Logger Logger { get; }
    
    private readonly IList<Type> _mixins = new List<Type>();

    public MixinManager(PluginMetadata pluginMetadata) : this(pluginMetadata.Id, pluginMetadata.Assembly) { }

    // ReSharper disable once ConvertToPrimaryConstructor
    public MixinManager(string id, Assembly? assembly = null) {
        Harmony = new HarmonyLib.Harmony(id);
        Assembly = assembly;
        Logger = BetterBeatSaber.Instance.Logger.GetChildLogger("MixinManager").GetChildLogger(id);
    }

    public void AddMixins() {
        
        var assembly = Assembly ?? new System.Diagnostics.StackTrace().GetFrame(1).GetMethod()?.DeclaringType?.Assembly;
        if (assembly == null) {
            Logger.Warn("Cannot register mixins for null assembly");
            return;
        }
        
        AddMixins(assembly);
        
    }

    public void AddMixins(Assembly assembly) =>
        AddMixins(assembly.GetTypes());

    public void AddMixins(IEnumerable<Type> types) {
        foreach (var type in types)
            if(IsValidMixin(type))
                AddMixin(type);
    }

    public void AddMixin(Type type) {
        if (IsValidMixin(type)) {
            _mixins.Add(type);
            Logger.Debug($"Registered Mixin {type.FullName}");
        } else {
            Logger.Warn("Cannot register a Type which is not an valid Mixin");
        }
    }
    
    public void Initialize() {
        try {
         
            var amount = 0;
        
            foreach (var type in _mixins) {
            
                var mixinAttribute = type.GetCustomAttribute<MixinAttribute>();
                if (mixinAttribute == null) {
                    var mixinPluginAttribute = type.GetCustomAttribute<MixinPluginAttribute>();
                    if (mixinPluginAttribute != null) {
                        mixinAttribute = mixinPluginAttribute;
                    } else {
                        Logger.Warn($"Type {type.FullName} does not have a MixinAttribute");
                        continue;
                    }
                }

                if (mixinAttribute.Type == null) {
                    if (mixinAttribute is MixinPluginAttribute mixinPluginAttribute) {
                        Logger.Info(mixinPluginAttribute.Plugin == null
                            ? $"Skipping Mixin {type.Name} because \"{mixinPluginAttribute.PluginId}\" is not installed"
                            : $"Skipping Mixin {type.Name} because the Type could not be found inside the Plugin");
                    } else {
                        Logger.Info($"Skipping Mixin {type.Name} because no Type was specified or found");
                    }
                    continue;
                }

                if (!mixinAttribute.ShouldRun()) {
                    Logger.Info($"Skipping Mixin {type.Name} because it should not run");
                    continue;
                }
            
                foreach (var method in type.GetMethods(PatchedMethodBindingFlags)) {
                    foreach (var customAttribute in method.GetCustomAttributes()) {
                    
                        if (customAttribute is not MixinMethodAttribute mixinMethodAttribute)
                            continue;
                 
                        var originalMethod = mixinAttribute.Type.GetMethods(OriginalMethodBindingFlags).FirstOrDefault(methodInfo => methodInfo.Name == mixinMethodAttribute.MethodName);
                        if (originalMethod == null) {
                            Logger.Warn($"Could not find method {mixinMethodAttribute.MethodName} in type {mixinAttribute.Type.FullName}");
                            continue;
                        }

                        if (!Patch(originalMethod, method, mixinMethodAttribute.At))
                            continue;
                    
                        Logger.Debug($"Ran Mixin for {mixinAttribute} ({mixinMethodAttribute.MethodName})");
                    
                        amount++;

                    }
                }
                
                // TODO: Add MixinProperties
            
            }
        
            Logger.Debug($"Ran {amount} Mixins");
            
        } catch (Exception exception) {
            Logger.Error("Failed to run Mixins");
            Logger.Error(exception);
        }
    }

    public void Exit() => Dispose();
    
    public void Dispose() {
        Harmony.UnpatchSelf();
        _mixins.Clear();
    }

    private bool Patch(MethodBase originalMethod, MethodInfo patchedMethod, MixinAt at) {
        try {
            
            switch (at) {
                case MixinAt.Pre:
                    Harmony.Patch(originalMethod, prefix: new HarmonyLib.HarmonyMethod(patchedMethod));
                    break;
                case MixinAt.Post:
                    Harmony.Patch(originalMethod, postfix: new HarmonyLib.HarmonyMethod(patchedMethod));
                    break;
                case MixinAt.Trans:
                    Harmony.Patch(originalMethod, transpiler: new HarmonyLib.HarmonyMethod(patchedMethod));
                    break;
                default:
                    return false;
            }

            return true;
            
        } catch (Exception exception) {
            Logger.Error($"Failed to patch method {originalMethod.Name} in {originalMethod.DeclaringType?.FullName}");
            Logger.Error(exception);
            return false;
        }
    }
    
    // ReSharper disable once SuggestBaseTypeForParameter
    private static bool IsValidMixin(Type type) =>
        type.GetCustomAttribute<MixinAttribute>() != null || type.GetCustomAttribute<MixinPluginAttribute>() != null;

}