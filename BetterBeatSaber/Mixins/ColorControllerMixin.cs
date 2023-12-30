using System;
using System.Linq;
using System.Reflection;

using BetterBeatSaber.Mixin;
using BetterBeatSaber.Utilities;

using IPA.Loader;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

//[MixinPlugin("ReeSabers", "ReeSabers.ColorController")]
internal static class ColorControllerMixin {

    private static PluginMetadata Plugin => PluginManager.GetPluginFromId("ReeSabers");
    
    private static Type _colorTransformType = null!;
    private static FieldInfo _observableField = null!;
    private static FieldInfo _isDirtyField = null!;
    private static MethodInfo _setValueMethod = null!;
    private static ConstructorInfo _hsbTransformConstructor = null!;

    private static bool _isInitialized;
    private static void Initialize() {

        var assembly = PluginManager.GetPluginFromId("ReeSabers").Assembly;
        
        _colorTransformType = assembly.GetType("ReeSabers.ColorTransformType");
        
        var colorControllerType = assembly.GetType("ReeSabers.ColorController");
        var observableValueType = assembly.GetType("ReeSabers.ObservableValue`1");
        var hsbTransformType = assembly.GetType("ReeSabers.HsbTransform");

        _observableField = colorControllerType.GetField("HsbTransform")!;
        _isDirtyField = colorControllerType.GetField("_isDirty", BindingFlags.Instance | BindingFlags.NonPublic)!;

        _setValueMethod = observableValueType.MakeGenericType(hsbTransformType).GetMethods().FirstOrDefault(method => method.Name == "SetValue" && method.GetParameters().Length == 2)!;

        _hsbTransformConstructor = hsbTransformType.GetConstructor(new[] { _colorTransformType, typeof(float), typeof(float), typeof(float), typeof(float) })!;

        _isInitialized = true;
        
    }
    
    [MixinMethod(nameof(Update), MixinAt.Post)]
    private static void Update(object __instance) {

        if(!_isInitialized)
            Initialize();

        var hsbTransform = _hsbTransformConstructor.Invoke(new[] { Enum.Parse(_colorTransformType, "HueOverride"), RGB.Instance.FirstHue, 1f, 0f, 1f })!;

        var observableValue = _observableField.GetValue(__instance)!;

        _setValueMethod.Invoke(observableValue, new[] { hsbTransform, __instance });
        
        _isDirtyField.SetValue(__instance, true);
        
    }
    
}