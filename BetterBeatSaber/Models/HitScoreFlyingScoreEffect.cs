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

    private static readonly Color Color112 = new(.3f, 0f, 1f);
    private static readonly Color Color110 = new(0f, 1f, 1f);
    private static readonly Color Color107 = new(0f, 1f, 0f);
    private static readonly Color Color105 = new(1f, .5f, 0f);
    private static readonly Color Color0 = new(1f, 0f, 0f);

    [UsedImplicitly]
    private Manager.ColorManager _colorManager = null!;
    
    private bool _accuracyMode;
    private bool _colorize;

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
            _text.ApplyGradient(_colorManager.FirstColor.WithAlpha(_alpha), _colorManager.ThirdColor.WithAlpha(_alpha));
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
        size = (int) (size * BetterBeatSaberConfig.Instance.HitScoreScale);

        _colorize = color == null;
        
        var text = $"<size={size}%>";
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

        if (BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.TimeDependency))
            text += $"\n<size=100%>{Mathf.Abs(cutScoreBuffer.noteCutInfo.cutNormal.z) * 100:n0}</size>";
        
        _text.text = text;
        
    }

    private static (Color?, int) GetColorAndSize(int score, int maxScore) =>
        maxScore switch {
            115 => score switch {
                115 => (null, 250),
                114 => (null, 225),
                >= 112 => (Color112, 200),
                >= 110 => (Color110, 175),
                >= 107 => (Color107, 162),
                >= 105 => (Color105, 150),
                _ => (Color0, 125)
            },
            85 => score switch {
                85 => (null, 225),
                83 => (null, 200),
                >= 80 => (Color112, 175),
                >= 70 => (Color110, 162),
                >= 60 => (Color107, 150),
                >= 50 => (Color105, 125),
                _ => (Color0, 125)
            },
            20 => score == 20 ? (null, 225) : (Color0, 200),
            _ => (null, 0)
        };

}