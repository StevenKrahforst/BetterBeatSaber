using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Utilities;

using HarmonyLib;

using IPA.Loader;
using IPA.Logging;

namespace BetterBeatSaber.Mixin;

public sealed class MixinManager : IDisposable {

    internal const BindingFlags OriginalMethodBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
    internal const BindingFlags PatchedMethodBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly;
    
    public Assembly? Assembly { get; }
    public Harmony Harmony { get; }

    private IList<Mixin> Mixins { get; } = new List<Mixin>();
    
    internal Logger Logger { get; }
    
    public MixinManager(PluginMetadata pluginMetadata) : this(pluginMetadata.Id, pluginMetadata.Assembly) { }

    // ReSharper disable once ConvertToPrimaryConstructor
    public MixinManager(string id, Assembly? assembly = null) {
        Harmony = new Harmony(id);
        Assembly = assembly;
        Logger = BetterBeatSaber.Instance.Logger.GetChildLogger("MixinManager").GetChildLogger(id);
    }

    #region Add

    public void AddMixins() {
        
        var assembly = Assembly ?? new StackTrace().GetFrame(1).GetMethod()?.DeclaringType?.Assembly;
        if (assembly == null) {
            Logger.Warn("Cannot register mixins for null assembly");
            return;
        }
        
        AddMixins(assembly);
        
    }

    public void AddMixins(Assembly assembly) =>
        AddMixins(assembly.GetTypes());

    public void AddMixins(IEnumerable<Type> types) {
        foreach (var type in types) {
            try {
                AddMixin(type);
            } catch(MixinException exception) {
                if (exception.Error != MixinError.MissingMixinAttribute) {
                    Logger.Error(exception.Message);
                    Logger.Debug(exception);
                }
            } catch (Exception exception) {
                Logger.Warn("Failed to register mixins");
                Logger.Error(exception);
            }
        }
    }

    private void AddMixin(Type type) {
        
        var mixinAttribute = type.GetCustomAttribute<MixinAttribute>();
        if (mixinAttribute == null)
            throw new MixinException(MixinError.MissingMixinAttribute, $"{type.FullName} is missing the MixinAttribute");
        
        var mixin = new Mixin(this, type, mixinAttribute.TypeResolver, mixinAttribute.ConflictsWith);
        
        var methods = new List<MixinMethod>();
        var actionHandlers = new List<MethodInfo>();
        
        foreach (var method in type.GetMethods(PatchedMethodBindingFlags)) {

            var mixinHandlerAttribute = method.GetCustomAttribute<MixinHandlerAttribute>();
            if (mixinHandlerAttribute != null) {
                actionHandlers.Add(method);
                continue;
            }
            
            foreach (var mixinMethodAttribute in method.GetCustomAttributes<MixinMethodAttribute>())
                methods.Add(new MixinMethod(this, mixin, mixinMethodAttribute.MethodName, method, mixinMethodAttribute.At, mixinMethodAttribute.ConflictsWith));
            
        }
        
        var toggleableMixinAttribute = type.GetCustomAttribute<ToggleableMixinAttribute>();
        if (toggleableMixinAttribute != null) {
            var observable = GetObservableBoolean(toggleableMixinAttribute.ConfigType, toggleableMixinAttribute.PropertyName);
            if (observable != null) {
                foreach (var mixinMethod in methods) {
                    mixinMethod.ShouldPatch = observable.CurrentValue;
                    mixinMethod.ListenToChanges(observable);
                }
            } else {
                Logger.Warn($"Could not find property {toggleableMixinAttribute.PropertyName} in type {toggleableMixinAttribute.ConfigType.FullName}");
            }
        }
        
        foreach (var mixinMethod in methods) {
            
            toggleableMixinAttribute = mixinMethod.PatchMethod.GetCustomAttribute<ToggleableMixinAttribute>();
            if(toggleableMixinAttribute == null)
                continue;
            
            var observable = GetObservableBoolean(toggleableMixinAttribute.ConfigType, toggleableMixinAttribute.PropertyName);
            if (observable == null) {
                Logger.Warn($"Could not find property {toggleableMixinAttribute.PropertyName} in type {toggleableMixinAttribute.ConfigType.FullName}");
                continue;
            }

            mixinMethod.ShouldPatch = observable.CurrentValue;
            mixinMethod.ListenToChanges(observable);
            
        }

        mixin.Methods = methods;
        mixin.ActionHandlers = actionHandlers;

        Mixins.Add(mixin);
        
        mixin.RunActionHandlers(MixinAction.Register);
        
        Logger.Debug($"Registered Mixin: {type.FullName} with {methods.Count} method{(methods.Count != 1 ? "s" : string.Empty)}");
        
    }
    
    #endregion
    
    #region Patch

    public void Patch() =>
        PatchMixins(Mixins);
    
    private void PatchMixins(IEnumerable<Mixin> mixins) {
        foreach (var mixin in mixins)
            PatchMixin(mixin);
    }

    private void PatchMixin(Mixin mixin) {
        foreach (var mixinMethod in mixin.Methods) {
            try {
                mixinMethod.Patch();
            } catch(MixinException exception) {
                Logger.Error(exception.Message);
                Logger.Debug(exception);
            } catch (Exception exception) {
                Logger.Warn("Failed to patch mixins");
                Logger.Error(exception);
            }
        }
    }

    #endregion

    #region Unpatch

    public void Unpatch() =>
        UnpatchMixins(Mixins);
    
    private void UnpatchMixins(IEnumerable<Mixin> mixins) {
        foreach (var mixin in mixins)
            UnpatchMixin(mixin);
    }

    private void UnpatchMixin(Mixin mixin) {
        foreach (var mixinMethod in mixin.Methods) {
            try {
                mixinMethod.Unpatch();
            } catch(MixinException exception) {
                Logger.Error(exception.Message);
                Logger.Debug(exception);
            } catch (Exception exception) {
                Logger.Warn("Failed to unpatch mixins");
                Logger.Error(exception);
            }
        }
    }

    #endregion
    
    public void Dispose() {
        Harmony.UnpatchSelf();
        Mixins.Clear();
    }

    private static ObservableValue<bool>? GetObservableBoolean(Type configType, string propertyName) =>
        (ObservableValue<bool>?) configType.GetProperty(propertyName)?.GetValue(BetterBeatSaberConfig.Instance); // Switch to ConfigManager get instance or typeof(Config) Instance
    
}