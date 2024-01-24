using BetterBeatSaber.Extensions;
using BetterBeatSaber.Providers;
using BetterBeatSaber.Utilities;

using TMPro;

using UnityEngine;

namespace BetterBeatSaber.Models; 

public sealed class HitScoreFlyingScoreEffect : FlyingScoreEffect {

    private static readonly Color Color112 = new(.3f, 0f, 1f);
    private static readonly Color Color110 = new(0f, 1f, 1f);
    private static readonly Color Color107 = new(0f, 1f, 0f);
    private static readonly Color Color105 = new(1f, .5f, 0f);
    private static readonly Color Color0 = new(1f, 0f, 0f);

    private NoteCutInfo? _noteCutInfo;
    private Colorizer? _colorizer;

    public override void InitAndPresent(IReadonlyCutScoreBuffer cutScoreBuffer, float duration, Vector3 targetPos, Color color) {
        
        _noteCutInfo = cutScoreBuffer.noteCutInfo;

        _color = color;
        _cutScoreBuffer = cutScoreBuffer;
        if (!cutScoreBuffer.isFinished) {
            cutScoreBuffer.RegisterDidChangeReceiver(this);
            cutScoreBuffer.RegisterDidFinishReceiver(this);
            _registeredToCallbacks = true;
        }

        if (_noteCutInfo!.Value.noteData.gameplayType is not NoteData.GameplayType.Normal) {
            _text.text = cutScoreBuffer.cutScore.ToString();
            _maxCutDistanceScoreIndicator.enabled = cutScoreBuffer.centerDistanceCutScore == cutScoreBuffer.noteScoreDefinition.maxCenterDistanceCutScore;
            _colorAMultiplier = cutScoreBuffer.cutScore > (double)cutScoreBuffer.maxPossibleCutScore * 0.9f ? 1f : 0.3f;
        } else {
            _maxCutDistanceScoreIndicator.enabled = false;

            // Apply judgments a total of twice - once when the effect is created, once when it finishes.
            Judge((CutScoreBuffer) cutScoreBuffer, 30);
        }

        InitAndPresent(duration, targetPos, cutScoreBuffer.noteCutInfo.worldRotation, false);
        
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

    public override void HandleCutScoreBufferDidChange(CutScoreBuffer cutScoreBuffer) {
        
        if (_noteCutInfo!.Value.noteData.gameplayType is not NoteData.GameplayType.Normal) {
            base.HandleCutScoreBufferDidChange(cutScoreBuffer);
            return;
        }

        Judge(cutScoreBuffer);
        
    }

    public override void HandleCutScoreBufferDidFinish(CutScoreBuffer cutScoreBuffer) {
        
        if (_noteCutInfo!.Value.noteData.gameplayType is NoteData.GameplayType.Normal) {
            Judge(cutScoreBuffer);
        }

        base.HandleCutScoreBufferDidFinish(cutScoreBuffer);
        
    }

    private void Judge(CutScoreBuffer cutScoreBuffer, int? assumedAfterCutScore = null) {

        _colorizer = gameObject.GetComponentInChildren<Colorizer>();
        if (_colorizer != null)
            Destroy(_colorizer);
        
        var before = cutScoreBuffer.beforeCutScore;
        var after = assumedAfterCutScore ?? cutScoreBuffer.afterCutScore;
        var accuracy = cutScoreBuffer.centerDistanceCutScore;
        
        var total = before + after + accuracy;

        Color? color = null;
        int size;
        switch (total) {
            case >= 115:
                size = 250;
                break;
            case >= 114:
                size = 225;
                break;
            case >= 112:
                size = 200;
                color = Color112;
                break;
            case >= 110:
                size = 175;
                color = Color110;
                break;
            case >= 107:
                size = 162;
                color = Color107;
                break;
            case >= 105:
                size = 150;
                color = Color105;
                break;
            default:
                size = 125;
                color = Color0;
                break;
        }

        size = (int) (size * BetterBeatSaberConfig.Instance.HitScoreScale);

        if(BetterBloomFontProvider.Instance != null)
            BetterBloomFontProvider.Instance.SetBloom(ref _text, BetterBeatSaberConfig.Instance.HitScoreGlow);
        
        _text.richText = true;

        if (BetterBeatSaberConfig.Instance.HitScoreTotalScore) {
            if (color.HasValue) {
                _text.text = $"<color=#{color.Value.ToHex()}><size={size}%>{total}</size></color>";
            } else {
                _text.text = $"<size={size}%>{total}</size>";
                _colorizer = _text.gameObject.AddComponent<Colorizer>();
                _colorizer.text = _text;
            }
        } else {
            if (color.HasValue) {
                _text.text = $"<color=#{color.Value.ToHex()}><size={size}%>{(before < 70 ? "<" : "")}{accuracy}{(after < 30 ? ">" : "")}</size></color>";
            } else {
                _text.text = $"<size={size}%>{(before < 70 ? "<" : "")}{accuracy}{(after < 30 ? ">" : "")}</size>";
                _colorizer = _text.gameObject.AddComponent<Colorizer>();
                _colorizer.text = _text;
            }
        }
        
    }

    private sealed class Colorizer : MonoBehaviour {

        public TextMeshPro? text;
        public float alpha;

        private void Update() {

            if (text == null)
                return;
            
            text.ForceMeshUpdate();
            
            var length = text.textInfo.characterInfo.Length;
            
            var steps = Steps(length);
            var gradients = new VertexGradient[length];
            for (var index = 0; index < length; index++) {

                gradients[index] = new VertexGradient(steps[index], steps[index + 1], steps[index], steps[index + 1]);
                    
                var characterInfo = text.textInfo.characterInfo[index];
                if (!characterInfo.isVisible || characterInfo.character == ' ')
                    continue;
                
                var colors = text.textInfo.meshInfo[characterInfo.materialReferenceIndex].colors32;
                
                var vertexIndex = text.textInfo.characterInfo[index].vertexIndex;
                
                colors[vertexIndex + 0] = gradients[index].bottomLeft;
                colors[vertexIndex + 1] = gradients[index].topLeft;
                colors[vertexIndex + 2] = gradients[index].bottomRight;
                colors[vertexIndex + 3] = gradients[index].topRight;
                
            }
            
            text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

        }
        
        private Color[] Steps(int amount) {
            
            amount += 2;
            
            var start = RGB.Instance.FirstColor.WithAlpha(alpha);
            var end = RGB.Instance.ThirdColor.WithAlpha(alpha);
            
            var result = new Color[amount];
            var r = (end.r - start.r) / (amount - 1);
            var g = (end.g - start.g) / (amount - 1);
            var b = (end.b - start.b) / (amount - 1);
            var a = (end.a - start.a) / (amount - 1);
            
            for (var index = 0; index < amount; index++)
                result[index] = new Color(start.r + r * index, start.g + g * index, start.b + b * index, start.a + a * index);
            
            return result;
            
        }

    }

}