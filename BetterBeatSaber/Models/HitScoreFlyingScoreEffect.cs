using BetterBeatSaber.Extensions;
using BetterBeatSaber.Providers;

using TMPro;

using UnityEngine;

namespace BetterBeatSaber.Models; 

internal sealed class HitScoreFlyingScoreEffect : FlyingScoreEffect {

    private static readonly Color Color112 = new(.3f, 0f, 1f);
    private static readonly Color Color110 = new(0f, 1f, 1f);
    private static readonly Color Color107 = new(0f, 1f, 0f);
    private static readonly Color Color105 = new(1f, .5f, 0f);
    private static readonly Color Color0 = new(1f, 0f, 0f);

    private Colorizer? _colorizer;

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

        Judge(cutScoreBuffer, 30);

        base.InitAndPresent(duration, targetPos, cutScoreBuffer.noteCutInfo.worldRotation, false);
        
    }

    protected override void ManualUpdate(float t) {
        var alpha = _fadeAnimationCurve.Evaluate(t);
        if (_colorizer != null) {
            _colorizer.alpha = alpha;
        } else {
            var color = _color.WithAlpha(alpha);
            _text.color = color;
            _maxCutDistanceScoreIndicator.color = color;
        }
    }

    public override void HandleCutScoreBufferDidChange(CutScoreBuffer cutScoreBuffer) =>
        Judge(cutScoreBuffer);

    public override void HandleCutScoreBufferDidFinish(CutScoreBuffer cutScoreBuffer) =>
        Judge(cutScoreBuffer);

    #endregion

    private void Judge(IReadonlyCutScoreBuffer cutScoreBuffer, int? assumedAfterCutScore = null) {

        _colorizer = gameObject.GetComponentInChildren<Colorizer>();
        
        if(!_text.richText)
            _text.richText = true;
        
        if(BetterBloomFontProvider.Instance != null)
            BetterBloomFontProvider.Instance.SetBloom(ref _text, BetterBeatSaberConfig.Instance.HitScoreBloom);
        
        var (color, size) = GetColorAndSize(cutScoreBuffer.cutScore, cutScoreBuffer.maxPossibleCutScore);
        size = (int) (size * BetterBeatSaberConfig.Instance.HitScoreScale);
        
        if (_colorizer != null && color != null) {
            DestroyImmediate(_colorizer);
            _colorizer = null;
        } else if (_colorizer == null && color == null) {
            _colorizer = _text.gameObject.AddComponent<Colorizer>();
            _colorizer.text = _text;
        }
        
        var text = $"<size={size}%>";
        if(color != null)
            text += $"<color=#{color.Value.ToHex()}>";
        
        var addAfterAndBefore = !BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.Accuracy) && cutScoreBuffer.noteCutInfo.noteData.gameplayType == NoteData.GameplayType.Normal && cutScoreBuffer.cutScore < cutScoreBuffer.maxPossibleCutScore;
        
        if(addAfterAndBefore && cutScoreBuffer.beforeCutScore < 70)
            text += "<";
        
        text += BetterBeatSaberConfig.Instance.HitScoreMode.HasFlag(HitScoreMode.Total) || (cutScoreBuffer.noteCutInfo.noteData.gameplayType != NoteData.GameplayType.Normal && cutScoreBuffer.noteCutInfo.noteData.gameplayType != NoteData.GameplayType.BurstSliderHead)
            ? cutScoreBuffer.cutScore
            : cutScoreBuffer.centerDistanceCutScore;
        
        if (addAfterAndBefore && (assumedAfterCutScore ?? cutScoreBuffer.afterCutScore) < 30)
            text += ">";
        
        if (color != null)
            text += "</color>";
        
        text += "</size>";
        
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

    private sealed class Colorizer : MonoBehaviour {

        public TextMeshPro? text;
        public float alpha = 1f;

        private void Update() {
            if (text != null && Manager.ColorManager.Instance != null)
                text.ApplyGradient(Manager.ColorManager.Instance.FirstColor.WithAlpha(alpha), Manager.ColorManager.Instance.ThirdColor.WithAlpha(alpha));
        }

    }

}