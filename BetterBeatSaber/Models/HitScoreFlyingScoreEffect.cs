using BetterBeatSaber.Enums;
using BetterBeatSaber.Extensions;
using BetterBeatSaber.Providers;

using JetBrains.Annotations;

using TMPro;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Models; 

/**
 * Support for 1.31+
 * 
 * https://github.com/daniel-chambers/HitScoreVisualizer/blob/update-1.32.0/HitScoreVisualizer/HarmonyPatches/FlyingScoreEffectPatch.cs
 * https://github.com/daniel-chambers/HitScoreVisualizer/blob/update-1.32.0/HitScoreVisualizer/HarmonyPatches/EffectPoolsManualInstallerPatch.cs
 */

internal sealed class HitScoreFlyingScoreEffect : FlyingScoreEffect {

    private static readonly Color White = new(1f, 1f, 1f);
    private static readonly Color Purple = new(.3f, 0f, 1f);
    private static readonly Color Cyan = new(0f, 1f, 1f);
    private static readonly Color Green = new(0f, 1f, 0f);
    private static readonly Color Yellow = new(1f, 1f, 0f);
    private static readonly Color Orange = new(1f, .5f, 0f);
    private static readonly Color Red = new(1f, 0f, 0f);

    [UsedImplicitly]
    private Manager.ColorManager _colorManager = null!;
    
    private bool _accuracyMode;
    private bool _colorize;
    private int _colorizationLength = -1;

    private float _alpha;

    [Inject, UsedImplicitly]
    internal void Construct(Manager.ColorManager colorManager) =>
        _colorManager = colorManager;
    
    #region Overrides

    public override void InitAndPresent(IReadonlyCutScoreBuffer cutScoreBuffer, float duration, Vector3 targetPos, Color color) {
        
        _color = color;
        _cutScoreBuffer = cutScoreBuffer;
        
        if (!cutScoreBuffer.isFinished) {
            cutScoreBuffer.RegisterDidChangeReceiver(this);
            cutScoreBuffer.RegisterDidFinishReceiver(this);
            _registeredToCallbacks = true;
        }
        
        _maxCutDistanceScoreIndicator.enabled = false;

        _accuracyMode = BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.Accuracy) && cutScoreBuffer.noteCutInfo.noteData.gameplayType is NoteData.GameplayType.Normal or NoteData.GameplayType.BurstSliderHead;
        
        Judge(cutScoreBuffer, 30);

        base.InitAndPresent(duration, targetPos, cutScoreBuffer.noteCutInfo.worldRotation, false);
        
    }

    protected override void ManualUpdate(float t) =>
        _alpha = _fadeAnimationCurve.Evaluate(t);
    
    public new void Update() {
        if (_colorize)
            _text.ApplyGradient(_colorManager.FirstColor.WithAlpha(_alpha), _colorManager.ThirdColor.WithAlpha(_alpha), _colorizationLength);
        else
            _text.alpha = _alpha;
        base.Update();
    }
    
    public override void HandleCutScoreBufferDidChange(CutScoreBuffer cutScoreBuffer) =>
        Judge(cutScoreBuffer);

    // Breaks the HSV, but not in Replays
    /*public override void HandleCutScoreBufferDidFinish(CutScoreBuffer cutScoreBuffer) =>
        Judge(cutScoreBuffer);*/

    #endregion

    private void ConfigureText() {
        
        if(!_text.richText)
            _text.richText = true;
        
        if(_text.enableWordWrapping)
            _text.enableWordWrapping = false;
        
        if(_text.overflowMode != TextOverflowModes.Overflow)
            _text.overflowMode = TextOverflowModes.Overflow;
        
        if(BetterBloomFontProvider.Instance != null)
            BetterBloomFontProvider.Instance.SetBloom(ref _text, BetterBeatSaberConfig.Instance.HitScoreBloom);
        
    }
    
    private void Judge(IReadonlyCutScoreBuffer cutScoreBuffer, int? assumedAfterCutScore = null) {

        ConfigureText();
        
        var (color, size) = GetColorAndSize(cutScoreBuffer.cutScore, cutScoreBuffer.maxPossibleCutScore);

        _colorize = color == null;
        
        var text = $"<size={size * BetterBeatSaberConfig.Instance.HitScoreScale:N0}%>";
        if(color != null)
            text += $"<color=#{color.Value.ToHex()}>";

        var addAfterAndBefore = _accuracyMode && cutScoreBuffer.cutScore < cutScoreBuffer.maxPossibleCutScore;
        
        if(addAfterAndBefore && cutScoreBuffer.beforeCutScore < 70)
            text += "<";

        text += _accuracyMode ? cutScoreBuffer.centerDistanceCutScore : cutScoreBuffer.cutScore;
        
        if (addAfterAndBefore && (assumedAfterCutScore ?? cutScoreBuffer.afterCutScore) < 30)
            text += ">";
        
        if (color != null)
            text += "</color>";
        
        text += "</size>";

        if (BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.TimeDependency)) {
            if (_colorize)
                _colorizationLength = text.Length - 1;
            var timeDependency = Mathf.Abs(cutScoreBuffer.noteCutInfo.cutNormal.z) * 100;
            text += $"\n<size={100 * BetterBeatSaberConfig.Instance.HitScoreScale:N0}%><color=#{GetTimeDependencyColor(timeDependency).ToHex()}>{timeDependency:N0}</color></size>";
        } else _colorizationLength = -1;
        
        _text.text = text;
        
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

}