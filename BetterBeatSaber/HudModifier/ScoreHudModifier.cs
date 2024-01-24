using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BetterBeatSaber.Providers;
using BetterBeatSaber.Utilities;

using JetBrains.Annotations;

using TMPro;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.HudModifier;

public sealed class ScoreHudModifier : HudModifier, IInitializable, ITickable, IDisposable {

    [UsedImplicitly]
    [Inject]
    private readonly RelativeScoreAndImmediateRankCounter _relativeScoreAndImmediateRankCounter = null!;

    [UsedImplicitly]
    [Inject]
    private readonly ImmediateRankUIPanel _immediateRankUIPanel = null!;

    [UsedImplicitly]
    [Inject]
    private readonly BetterBloomFontProvider _bloomFontProvider = null!;
    
    private TextMeshProUGUI? _rankText;
    private TextMeshProUGUI? _scoreText;

    private TMP_FontAsset? _rankTextDefaultFont;
    private TMP_FontAsset? _scoreTextDefaultFont;
    
    private bool _rgb;
    private bool _gradient;
    private Color _color;
    private Color _secondColor;

    private List<Rank> _ranks;

    public void Initialize() {

        _rankText = (TextMeshProUGUI?) typeof(ImmediateRankUIPanel).GetField("_rankText", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_immediateRankUIPanel);
        _scoreText = (TextMeshProUGUI?) typeof(ImmediateRankUIPanel).GetField("_relativeScoreText", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_immediateRankUIPanel);

        if(_rankText != null)
            _rankTextDefaultFont = _rankText.font;
        
        if(_scoreText != null)
            _scoreTextDefaultFont = _scoreText.font;

        _relativeScoreAndImmediateRankCounter.relativeScoreOrImmediateRankDidChangeEvent += OnRelativeScoreOrImmediateRankDidChangeEvent;

        _ranks = BetterBeatSaberConfig.Instance.ScoreHudModifier.Ranks;
        _ranks.Sort((rank1, rank2) => rank2.Threshold.CompareTo(rank1.Threshold));
        
        UpdateS(1f);

    }

    public void Tick() {

        var firstColor = _rgb ? RGB.Instance.FirstColor : _color;
        var secondColor = _rgb ? RGB.Instance.SecondColor : _secondColor;
        
        if (_gradient) {
            
            if(_rankText != null)
                ApplyGradient(ref _rankText, firstColor, secondColor);
                
            if(_scoreText != null)
                ApplyGradient(ref _scoreText, firstColor, secondColor);
            
        } else {

            if (_rankText != null)
                _rankText.color = firstColor;
            
            if (_scoreText != null)
                _scoreText.color = firstColor;

        }
    }

    private static void ApplyGradient(ref TextMeshProUGUI text, Color start, Color end) {
        
        text.ForceMeshUpdate();
            
        var length = text.textInfo.characterInfo.Length;
        
        var steps = Steps(length, start, end);
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
    
    public void Dispose() =>
        _relativeScoreAndImmediateRankCounter.relativeScoreOrImmediateRankDidChangeEvent -= OnRelativeScoreOrImmediateRankDidChangeEvent;

    private void OnRelativeScoreOrImmediateRankDidChangeEvent() =>
        UpdateS(_relativeScoreAndImmediateRankCounter.relativeScore);

    private void UpdateS(float score) {
        
        var rank = _ranks.FirstOrDefault(rank => score >= rank.Threshold) ?? _ranks.Last();
        
        _gradient = rank.ColorMode is ColorMode.ColorGradient or ColorMode.RGBGradient;
        _rgb = rank.ColorMode is ColorMode.RGB or ColorMode.RGBGradient;
        
        _color = rank.Color;
        _secondColor = rank.SecondColor;

        if (_rankText != null) {
            _rankText.font = rank.Bloom ? _bloomFontProvider.BloomFont : _rankTextDefaultFont;
            _rankText.text = rank.Name;
        }
        
        if(_scoreText != null)
            _scoreText.font = rank.Bloom ? _bloomFontProvider.BloomFont : _scoreTextDefaultFont;
        
    }
    
    private static Color[] Steps(int amount, Color start, Color end) {
        
        amount += 2;
        
        var result = new Color[amount];
        var r = (end.r - start.r) / (amount - 1);
        var g = (end.g - start.g) / (amount - 1);
        var b = (end.b - start.b) / (amount - 1);
        var a = (end.a - start.a) / (amount - 1);
        
        for (var index = 0; index < amount; index++)
            result[index] = new Color(start.r + r * index, start.g + g * index, start.b + b * index, start.a + a * index);
        
        return result;
        
    }

    public sealed class Options : BaseOptions {

        public List<Rank> Ranks { get; } = [
            new Rank {
                Threshold = .95f,
                Name = "UwU",
                ColorMode = ColorMode.RGBGradient,
                Color = Color.red, // LOL
                Bloom = true
            },
            new Rank {
                Threshold = .9f,
                Name = "SUS",
                ColorMode = ColorMode.RGB,
                Color = new Color(0f, .5f, .5f),
                Bloom = true
            },
            new Rank {
                Threshold = .8f,
                Name = "OK",
                Color = new Color(1f, 1f, 1f),
                SecondColor = new Color(.95f, .95f, .95f),
                ColorMode = ColorMode.ColorGradient,
                Bloom = true
            },
            new Rank {
                Threshold = .7f,
                Name = "MHH",
                Color = new Color(0f, 1f, 0f),
                Bloom = true
            },
            new Rank {
                Threshold = .6f,
                Name = "DEAD",
                Color = new Color(1f, 0f, 0f),
                Bloom = true
            }
        ];

    }
    
    public sealed class Rank {

        public float Threshold { get; set; }
        public string Name { get; set; } = null!;
        public ColorMode ColorMode { get; set; } = ColorMode.Color;
        public Color Color { get; set; } = Extensions.ColorExtensions.None;
        public Color SecondColor { get; set; } = Extensions.ColorExtensions.None;
        public bool Bloom { get; set; }

    }
    
    public enum ColorMode {

        RGB,
        RGBGradient,
        Color,
        ColorGradient

    }

}