using System.Collections.Generic;

using BetterBeatSaber.Enums;
using BetterBeatSaber.Extensions;
using BetterBeatSaber.Mixin.Attributes;
using BetterBeatSaber.Mixin.Enums;

using IPA.Utilities;

using TMPro;

using UnityEngine;

namespace BetterBeatSaber.Mixins;

/**
 * Support for 1.31+
 *
 * https://github.com/daniel-chambers/HitScoreVisualizer/blob/update-1.32.0/HitScoreVisualizer/HarmonyPatches/FlyingScoreEffectPatch.cs
 * https://github.com/daniel-chambers/HitScoreVisualizer/blob/update-1.32.0/HitScoreVisualizer/HarmonyPatches/EffectPoolsManualInstallerPatch.cs
 */

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

// ReSharper disable RedundantAssignment

[Mixin(typeof(FlyingScoreEffect))]
internal static class FlyingScoreEffectMixin {

    #region Constants
    
    private static readonly Color White = new(1f, 1f, 1f);
    private static readonly Color Purple = new(.3f, 0f, 1f);
    private static readonly Color Cyan = new(0f, 1f, 1f);
    private static readonly Color Green = new(0f, 1f, 0f);
    private static readonly Color Yellow = new(1f, 1f, 0f);
    private static readonly Color Orange = new(1f, .5f, 0f);
    private static readonly Color Red = new(1f, 0f, 0f);

    #endregion

    private static readonly FieldAccessor<FlyingScoreEffect, TextMeshPro>.Accessor TextAccessor = FieldAccessor<FlyingScoreEffect, TextMeshPro>.GetAccessor("_text");
    
    private static readonly Dictionary<FlyingScoreEffect, FlyingScoreEffectData> Data = new();

    [MixinMethod(nameof(InitAndPresent), MixinAt.Pre)]
    private static bool InitAndPresent(
        FlyingScoreEffect __instance,
        IReadonlyCutScoreBuffer cutScoreBuffer,
        Color color,
        float duration,
        Vector3 targetPos,
        ref Color ____color,
        ref IReadonlyCutScoreBuffer ____cutScoreBuffer,
        ref bool ____registeredToCallbacks,
        ref SpriteRenderer ____maxCutDistanceScoreIndicator,
        ref TextMeshPro ____text) {
        
        ____color = color;
        ____cutScoreBuffer = cutScoreBuffer;
        
        if (!cutScoreBuffer.isFinished) {
            cutScoreBuffer.RegisterDidChangeReceiver(__instance);
            cutScoreBuffer.RegisterDidFinishReceiver(__instance);
            ____registeredToCallbacks = true;
        }
        
        ____maxCutDistanceScoreIndicator.enabled = false;

        if (Data.TryGetValue(__instance, out var value)) {
            value.AccuracyMode = BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.Accuracy) && cutScoreBuffer.noteCutInfo.noteData.gameplayType is NoteData.GameplayType.Normal or NoteData.GameplayType.BurstSliderHead;
        } else {
            Data[__instance] = new FlyingScoreEffectData {
                Colorize = false,
                ColorizationLength = -1,
                Alpha = 1f,
                AccuracyMode = BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.Accuracy) && cutScoreBuffer.noteCutInfo.noteData.gameplayType is NoteData.GameplayType.Normal or NoteData.GameplayType.BurstSliderHead
            };
        }

        Judge(__instance, ____text, cutScoreBuffer, 30);

        __instance.InitAndPresent(duration, targetPos, cutScoreBuffer.noteCutInfo.worldRotation, false);
        
        return false;

    }
    
    [MixinMethod(nameof(Update), MixinAt.Pre)]
    private static void Update(FlyingScoreEffect __instance) {
        var text = TextAccessor(ref __instance);
        if (!Data.TryGetValue(__instance, out var data))
            return;
        if (Data[__instance].Colorize)
            text.ApplyGradient(Manager.ColorManager.Instance.FirstColor.WithAlpha(data.Alpha), Manager.ColorManager.Instance.ThirdColor.WithAlpha(data.Alpha), data.ColorizationLength);
        else
            text.alpha = data.Alpha;
    }
    
    [MixinMethod(nameof(ManualUpdate), MixinAt.Pre)]
    private static bool ManualUpdate(FlyingScoreEffect __instance, float t, ref AnimationCurve ____fadeAnimationCurve) {
        if(Data.TryGetValue(__instance, out var data))
            data.Alpha = ____fadeAnimationCurve.Evaluate(t);
        return false;
    }

    [MixinMethod(nameof(HandleCutScoreBufferDidChange), MixinAt.Pre)]
    private static bool HandleCutScoreBufferDidChange(FlyingScoreEffect __instance, CutScoreBuffer cutScoreBuffer, ref TextMeshPro ____text) {
        Judge(__instance, ____text, cutScoreBuffer);
        return false;
    }

    [MixinMethod(nameof(HandleCutScoreBufferDidFinish), MixinAt.Pre)]
    private static bool HandleCutScoreBufferDidFinish() => true;
    
    private static void Judge(FlyingScoreEffect flyingScoreEffect, TextMeshPro textMeshPro, IReadonlyCutScoreBuffer cutScoreBuffer, int? assumedAfterCutScore = null) {

        ConfigureText(ref textMeshPro);
        
        var (color, size) = GetColorAndSize(cutScoreBuffer.cutScore, cutScoreBuffer.maxPossibleCutScore);

        Data[flyingScoreEffect].Colorize = color == null;
        
        var text = $"<size={size * BetterBeatSaberConfig.Instance.HitScoreScale:N0}%>";
        if(color != null)
            text += $"<color=#{color.Value.ToHex()}>";

        var addAfterAndBefore = Data[flyingScoreEffect].AccuracyMode && cutScoreBuffer.cutScore < cutScoreBuffer.maxPossibleCutScore;
        
        if(addAfterAndBefore && cutScoreBuffer.beforeCutScore < 70)
            text += "<";

        text += Data[flyingScoreEffect].AccuracyMode ? cutScoreBuffer.centerDistanceCutScore : cutScoreBuffer.cutScore;
        
        if (addAfterAndBefore && (assumedAfterCutScore ?? cutScoreBuffer.afterCutScore) < 30)
            text += ">";
        
        if (color != null)
            text += "</color>";
        
        text += "</size>";

        if (BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.TimeDependency)) {
            if (Data[flyingScoreEffect].Colorize)
                Data[flyingScoreEffect].ColorizationLength = text.Length - 1;
            var timeDependency = Mathf.Abs(cutScoreBuffer.noteCutInfo.cutNormal.z) * 100;
            text += $"\n<size={100 * BetterBeatSaberConfig.Instance.HitScoreScale:N0}%><color=#{GetTimeDependencyColor(timeDependency).ToHex()}>{timeDependency:N0}</color></size>";
        } else Data[flyingScoreEffect].ColorizationLength = -1;
        
        textMeshPro.text = text;
        
    }
    
    private static void ConfigureText(ref TextMeshPro text) {
        
        if(!text.richText)
            text.richText = true;
        
        if(text.enableWordWrapping)
            text.enableWordWrapping = false;
        
        if(text.overflowMode != TextOverflowModes.Overflow)
            text.overflowMode = TextOverflowModes.Overflow;

        text.SetBloom(BetterBeatSaberConfig.Instance.HitScoreBloom);
        
    }

    private static (Color?, int) GetColorAndSize(int score, int maxScore) =>
        maxScore switch {
            115 => score switch {
                115 => (null, 250),
                114 => (null, 225),
                >= 112 => (Purple, 200),
                >= 110 => (Cyan, 175),
                >= 107 => (Green, 162),
                >= 105 => (Orange, 150),
                _ => (Red, 125)
            },
            85 => score switch {
                85 => (null, 225),
                83 => (null, 200),
                >= 80 => (Purple, 175),
                >= 70 => (Cyan, 162),
                >= 60 => (Green, 150),
                >= 50 => (Orange, 125),
                _ => (Red, 125)
            },
            20 => score == 20 ? (null, 225) : (Red, 200),
            _ => (null, 0)
        };
    
    private static Color GetTimeDependencyColor(float score) =>
        score switch {
            >= 21 => Red,
            >= 11 => Orange,
            >= 6 => Yellow,
            _ => White
        };

    // ReSharper disable once ClassNeverInstantiated.Local
    private record FlyingScoreEffectData {

        public bool AccuracyMode { get; set; }
        public float Alpha { get; set; }
        public bool Colorize { get; set; }
        public int ColorizationLength { get; set; }
        
    }
    
}