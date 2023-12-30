using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using IPA.Logging;

using Logger = IPA.Logging.Logger;

namespace BetterBeatSaber.Extensions; 

public static class ReflectionExtensions {

    private static Logger? _logger;
    private static Logger Logger => _logger ??= BetterBeatSaber.Instance.Logger.GetChildLogger("Reflections");

    public const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy;

    #region Type

    public static T? Construct<T>(this Type type, IEnumerable<object>? constructorParameters = null, IDictionary<string, object>? injections = null) =>
        (T?) Construct(type, constructorParameters, injections);

    public static object? Construct(this Type type, IEnumerable<object>? constructorParameters = null, IDictionary<string, object>? injections = null) {
        
        var constructor = constructorParameters != null
            // ReSharper disable once PossibleMultipleEnumeration
            ? type.GetConstructor(DefaultBindingFlags, null, constructorParameters.Select(parameter => parameter.GetType()).ToArray(), null)
            : type.GetConstructors(DefaultBindingFlags).FirstOrDefault();
        
        if (constructor == null) {
            Logger.Warn($"Failed to find a matching Constructor when building {type.FullName}");
            return null;
        }
        
        // ReSharper disable once PossibleMultipleEnumeration
        var instance = constructor.Invoke(constructorParameters?.ToArray() ?? Array.Empty<object>());
        if (instance == null) {
            Logger.Warn($"Failed to instantiate class when building {type.FullName}");
            return null;
        }

        if (injections == null || injections.Count == 0)
            return instance;
            
        foreach (var injection in injections) {

            var property = type.GetProperty(injection.Key, DefaultBindingFlags);
            if (property != null) {
                property.SetValue(instance, injection.Value);
                continue;
            }
            
            var field = type.GetField(injection.Key, DefaultBindingFlags);
            if (field != null)
                field.SetValue(instance, injection.Value);
            
        }
        
        return instance;
        
    }
    
    public static T? GetInstance<T>(this Type type) =>
        (T?) type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)!.GetValue(null);

    #endregion

    #region Object

    #region Field

    public static void SetField<T>(this T instance, string name, object? value, BindingFlags bindingFlags = DefaultBindingFlags) =>
        typeof(T).GetField(name, bindingFlags)?.SetValue(instance, value);

    public static TValue? GetField<TValue, T>(this T instance, string name, BindingFlags bindingFlags = DefaultBindingFlags) =>
        (TValue?) typeof(T).GetField(name, bindingFlags)?.GetValue(instance);

    #endregion
    
    #region Property

    public static void SetProperty<T>(this T instance, string name, object? value, BindingFlags bindingFlags = DefaultBindingFlags) =>
        typeof(T).GetProperty(name, bindingFlags)?.SetValue(instance, value);

    public static TValue? GetProperty<TValue, T>(this T instance, string name, BindingFlags bindingFlags = DefaultBindingFlags) =>
        (TValue?) typeof(T).GetProperty(name, bindingFlags)?.GetValue(instance);

    #endregion

    #endregion
    
}