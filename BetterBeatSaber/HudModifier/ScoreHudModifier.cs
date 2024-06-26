﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BetterBeatSaber.Extensions;

using JetBrains.Annotations;

using TMPro;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.HudModifier;

internal sealed class ScoreHudModifier : IHudModifier, ITickable, IDisposable {

    [Inject, UsedImplicitly]
    private readonly RelativeScoreAndImmediateRankCounter _relativeScoreAndImmediateRankCounter = null!;

    [Inject, UsedImplicitly]
    private readonly ImmediateRankUIPanel _immediateRankUIPanel = null!;

    private TextMeshProUGUI? _rankText;
    private TextMeshProUGUI? _scoreText;

    private TMP_FontAsset? _rankTextDefaultFont;
    private TMP_FontAsset? _scoreTextDefaultFont;
    
    private bool _rgb;
    private bool _gradient;
    private Color _color;
    private Color _secondColor;

    private List<Rank> _ranks = null!;

    public void Initialize() {

        _rankText = (TextMeshProUGUI?) typeof(ImmediateRankUIPanel).GetField("_rankText", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_immediateRankUIPanel);
        _scoreText = (TextMeshProUGUI?) typeof(ImmediateRankUIPanel).GetField("_relativeScoreText", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_immediateRankUIPanel);

        if(_rankText != null)
            _rankTextDefaultFont = _rankText.font;
        
        if(_scoreText != null)
            _scoreTextDefaultFont = _scoreText.font;

        _relativeScoreAndImmediateRankCounter.relativeScoreOrImmediateRankDidChangeEvent += OnRelativeScoreOrImmediateRankDidChangeEvent;

        _ranks = BetterBeatSaberConfig.Instance.ScoreHudModifier.Ranks.ToList();
        _ranks.Sort((rank1, rank2) => rank2.Threshold.CompareTo(rank1.Threshold));
        
        UpdateS(1f);

    }

    public void Tick() {

        var firstColor = _rgb ? Manager.ColorManager.Instance.FirstColor : _color;
        var secondColor = _rgb ? Manager.ColorManager.Instance.SecondColor : _secondColor;
        
        if (_gradient) {

            if (_rankText != null)
                _rankText.ApplyGradient(firstColor, secondColor);
            
            if (_scoreText != null)
                _scoreText.ApplyGradient(firstColor, secondColor);
            
        } else {

            if (_rankText != null)
                _rankText.color = firstColor;
            
            if (_scoreText != null)
                _scoreText.color = firstColor;

        }
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
            _rankText.font = rank.Bloom ? TextMeshProExtensions.BloomFont : _rankTextDefaultFont;
            _rankText.text = rank.Name;
        }
        
        if(_scoreText != null)
            _scoreText.font = rank.Bloom ? TextMeshProExtensions.BloomFont : _scoreTextDefaultFont;
        
    }
    
    public sealed class Options : BetterBeatSaberConfig.HudModifierOptions {

        public Rank[] Ranks { get; set; } = [
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