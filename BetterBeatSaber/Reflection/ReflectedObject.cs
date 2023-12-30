using System;
using System.Linq;
using System.Reflection;

namespace BetterBeatSaber.Reflection;

public sealed class ReflectedObject {
    
    public const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    public Type Type => Object.GetType();
    public object Object { get; set; }
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public ReflectedObject(object obj) {
        Object = obj;
    }

    public ReflectedObject(Type type, params object[] args) : this(type, DefaultBindingFlags, args) {}
    
    public ReflectedObject(Type type, BindingFlags bindingFlags, params object[] args) {
        var constructor = type.GetConstructors(bindingFlags).FirstOrDefault(constructor => constructor.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(args.Select(arg => arg.GetType())));
        Object = constructor?.Invoke(args)!;
    }
    
    #region Method

    public MethodInfo? GetMethod(string methodName, BindingFlags bindingFlags = DefaultBindingFlags) =>
        Type.GetMethod(methodName, bindingFlags);
    
    public MethodInfo? GetMethod(string methodName, BindingFlags bindingFlags = DefaultBindingFlags, params object[] args) =>
        Type.GetMethods(bindingFlags).FirstOrDefault(method => method.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(args.Select(arg => arg.GetType())));
    
    public void InvokeMethod(string methodName, params object[] args) =>
        InvokeMethod(methodName, DefaultBindingFlags, args);
    
    public void InvokeMethod(string methodName, BindingFlags bindingFlags, params object[] args) =>
        GetMethod(methodName, bindingFlags, args)?.Invoke(Object, args);
    
    #endregion
    
    #region Field

    public void SetFieldValue<T>(string fieldName, T value, BindingFlags bindingFlags = DefaultBindingFlags) =>
        Type.GetField(fieldName, bindingFlags)?.SetValue(Object, value);

    public T? GetFieldValue<T>(string fieldName, BindingFlags bindingFlags = DefaultBindingFlags) =>
        (T?) GetFieldValue(fieldName, bindingFlags);

    public object? GetFieldValue(string fieldName, BindingFlags bindingFlags = DefaultBindingFlags) =>
        Type.GetField(fieldName, bindingFlags)?.GetValue(Object);
    
    #endregion

    #region Property

    #endregion
    
    public static ReflectedObject From(object obj) => new(obj);

    public static ReflectedObject Create<T>(params object[] args) =>
        Create(typeof(T), args);
    
    public static ReflectedObject Create(Type type, params object[] args) =>
        new(type, args);

}