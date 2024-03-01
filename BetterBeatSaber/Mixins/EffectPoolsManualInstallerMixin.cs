using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;
using BetterBeatSaber.Models;

using HarmonyLib;

using IPA.Utilities;

using TMPro;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(EffectPoolsManualInstaller), "HitScoreVisualizer")]
[ToggleableMixin(typeof(BetterBeatSaberConfig), nameof(BetterBeatSaberConfig.HitScoreEnable))]
internal static class EffectPoolsManualInstallerMixin {
	
	private static readonly MethodInfo MemoryPoolBinderOriginal = typeof(DiContainer).GetMethods().First(x => x.Name == nameof(DiContainer.BindMemoryPool) && x.IsGenericMethod && x.GetGenericArguments().Length == 2).MakeGenericMethod(typeof(FlyingScoreEffect), typeof(FlyingScoreEffect.Pool));
    private static readonly MethodInfo MemoryPoolBinderReplacement = SymbolExtensions.GetMethodInfo(() => MemoryPoolBinderStub(null!));

    private static readonly MethodInfo WithInitialSizeOriginal = typeof(MemoryPoolInitialSizeMaxSizeBinder<FlyingScoreEffect>).GetMethod(nameof(MemoryPoolInitialSizeMaxSizeBinder<FlyingScoreEffect>.WithInitialSize), new[] { typeof(int) })!;
    private static readonly MethodInfo WithInitialSizeReplacement = SymbolExtensions.GetMethodInfo(() => PoolSizeDefinitionStub(null!, 0));
    
    internal static readonly FieldAccessor<FlyingObjectEffect, AnimationCurve>.Accessor MoveAnimationCurveAccessor = FieldAccessor<FlyingObjectEffect, AnimationCurve>.GetAccessor("_moveAnimationCurve");
    internal static readonly FieldAccessor<FlyingObjectEffect, float>.Accessor ShakeFrequencyAccessor = FieldAccessor<FlyingObjectEffect, float>.GetAccessor("_shakeFrequency");
    internal static readonly FieldAccessor<FlyingObjectEffect, float>.Accessor ShakeStrengthAccessor = FieldAccessor<FlyingObjectEffect, float>.GetAccessor("_shakeStrength");
    internal static readonly FieldAccessor<FlyingObjectEffect, AnimationCurve>.Accessor ShakeStrengthAnimationCurveAccessor = FieldAccessor<FlyingObjectEffect, AnimationCurve>.GetAccessor("_shakeStrengthAnimationCurve");
    internal static readonly FieldAccessor<FlyingScoreEffect, TextMeshPro>.Accessor TextAccessor = FieldAccessor<FlyingScoreEffect, TextMeshPro>.GetAccessor("_text");
    internal static readonly FieldAccessor<FlyingScoreEffect, AnimationCurve>.Accessor FadeAnimationCurveAccessor = FieldAccessor<FlyingScoreEffect, AnimationCurve>.GetAccessor("_fadeAnimationCurve");
    internal static readonly FieldAccessor<FlyingScoreEffect, SpriteRenderer>.Accessor SpriteRendererAccessor = FieldAccessor<FlyingScoreEffect, SpriteRenderer>.GetAccessor("_maxCutDistanceScoreIndicator");

    [MixinMethod("ManualInstallBindings", MixinAt.Pre)]
    // ReSharper disable once SuggestBaseTypeForParameter
    private static void Prefix(FlyingScoreEffect ____flyingScoreEffectPrefab) {
        
	    var gameObject = ____flyingScoreEffectPrefab.gameObject;

	    var flyingScoreEffect = gameObject.GetComponent<FlyingScoreEffect>();
	    flyingScoreEffect.enabled = false;
	    
	    var hsvScoreEffect = gameObject.GetComponent<HitScoreFlyingScoreEffect>();
	    if (hsvScoreEffect)
		    return;
	    
	    var hsvFlyingScoreEffect = gameObject.AddComponent<HitScoreFlyingScoreEffect>();

	    // Serialized fields aren't filled in correctly in our own custom override, so copying over the values using FieldAccessors
	    var flyingObjectEffect = (FlyingObjectEffect) flyingScoreEffect;
	    
	    FieldAccessor<FlyingObjectEffect, AnimationCurve>.Set(hsvFlyingScoreEffect, "_moveAnimationCurve", MoveAnimationCurveAccessor(ref flyingObjectEffect));
	    FieldAccessor<FlyingObjectEffect, float>.Set(hsvFlyingScoreEffect, "_shakeFrequency", ShakeFrequencyAccessor(ref flyingObjectEffect));
	    FieldAccessor<FlyingObjectEffect, float>.Set(hsvFlyingScoreEffect, "_shakeStrength", ShakeStrengthAccessor(ref flyingObjectEffect));
	    FieldAccessor<FlyingObjectEffect, AnimationCurve>.Set(hsvFlyingScoreEffect, "_shakeStrengthAnimationCurve", ShakeStrengthAnimationCurveAccessor(ref flyingObjectEffect));

	    FieldAccessor<FlyingScoreEffect, TextMeshPro>.Set(hsvFlyingScoreEffect, "_text", TextAccessor(ref flyingScoreEffect));
	    FieldAccessor<FlyingScoreEffect, AnimationCurve>.Set(hsvFlyingScoreEffect, "_fadeAnimationCurve", FadeAnimationCurveAccessor(ref flyingScoreEffect));
	    FieldAccessor<FlyingScoreEffect, SpriteRenderer>.Set(hsvFlyingScoreEffect, "_maxCutDistanceScoreIndicator", SpriteRendererAccessor(ref flyingScoreEffect));
	    
	}

	[MixinMethod("ManualInstallBindings", MixinAt.Trans)]
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
		return new CodeMatcher(instructions)
			.MatchForward(false,
				new CodeMatch(OpCodes.Ldarg_1), // push DiContainer instance (first method parameter) onto the evaluation stack
				new CodeMatch(OpCodes.Callvirt, MemoryPoolBinderOriginal), // Call method BindMemoryPool<,>()
				new CodeMatch(OpCodes.Ldc_I4_S), // push InitialSize parameter with value X onto the evaluation stack
				new CodeMatch(OpCodes.Callvirt, WithInitialSizeOriginal)) // Call method WithInitialSize(size)
			.Advance(1)
			.SetOperandAndAdvance(MemoryPoolBinderReplacement)
			.Advance(1)
			.SetOperandAndAdvance(WithInitialSizeReplacement)
			.InstructionEnumeration();
	}

	// ReSharper disable once UnusedMethodReturnValue.Local
	private static MemoryPoolIdInitialSizeMaxSizeBinder<HitScoreFlyingScoreEffect> MemoryPoolBinderStub(DiContainer contract) {
		return contract.BindMemoryPool<HitScoreFlyingScoreEffect, FlyingScoreEffect.Pool>();
	}

	// ReSharper disable once UnusedMethodReturnValue.Local
	private static MemoryPoolMaxSizeBinder<HitScoreFlyingScoreEffect> PoolSizeDefinitionStub(MemoryPoolIdInitialSizeMaxSizeBinder<HitScoreFlyingScoreEffect> contract, int initialSize) {
		return contract.WithInitialSize(initialSize);
	}

}