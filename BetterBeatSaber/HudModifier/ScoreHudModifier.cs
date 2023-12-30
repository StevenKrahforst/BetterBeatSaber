using System;
using System.Collections.Generic;
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
    private readonly BloomFontProvider _bloomFontProvider = null!;
    
    private TextMeshProUGUI? _rankText;
    private TextMeshProUGUI? _scoreText;

    private TMP_FontAsset? _rankTextDefaultFont;
    private TMP_FontAsset? _scoreTextDefaultFont;
    
    private bool _rgb = true;

    public void Initialize() {

        _rankText = (TextMeshProUGUI?) typeof(ImmediateRankUIPanel).GetField("_rankText", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_immediateRankUIPanel);
        _scoreText = (TextMeshProUGUI?) typeof(ImmediateRankUIPanel).GetField("_relativeScoreText", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_immediateRankUIPanel);

        if(_rankText != null)
            _rankTextDefaultFont = _rankText.font;
        
        if(_scoreText != null)
            _scoreTextDefaultFont = _scoreText.font;

        _relativeScoreAndImmediateRankCounter.relativeScoreOrImmediateRankDidChangeEvent += OnRelativeScoreOrImmediateRankDidChangeEvent;

        OnRelativeScoreOrImmediateRankDidChangeEvent();
        
    }

    public void Tick() {
        
        if (!_rgb)
            return;
        
        if(_rankText != null)
            _rankText.color = RGB.Instance.FirstColor;
        
        if(_scoreText != null)
            _scoreText.color = RGB.Instance.FirstColor;
        
    }
    
    public void Dispose() {
        _relativeScoreAndImmediateRankCounter.relativeScoreOrImmediateRankDidChangeEvent -= OnRelativeScoreOrImmediateRankDidChangeEvent;
    }

    private void OnRelativeScoreOrImmediateRankDidChangeEvent() {
        
        var immediateRank = _relativeScoreAndImmediateRankCounter.immediateRank;

        if (!BetterBeatSaberConfig.Instance.ScoreHudModifier.CustomRanks.ContainsKey(immediateRank) || !BetterBeatSaberConfig.Instance.ScoreHudModifier.CustomRanks.TryGetValue(immediateRank, out var config) || !config.Enable)
            return;

        if(_rankText != null)
            _rankText.font = config.Glow ? _bloomFontProvider.BloomFont : _rankTextDefaultFont;
        
        if(_scoreText != null)
            _scoreText.font = config.Glow ? _bloomFontProvider.BloomFont : _scoreTextDefaultFont;
        
        _rgb = config.RGB;
        
        if (_rgb)
            return;
        
        if(_rankText != null)
            _rankText.color = config.Color;
        
        if(_scoreText != null)
            _scoreText.color = config.Color;

    }

    public class Options : BaseOptions {

        public Dictionary<RankModel.Rank, RankConfig> CustomRanks { get; set; } = new() {
            {
                RankModel.Rank.SS, new RankConfig {
                    Enable = true,
                    Name = "SUS",
                    Color = new Color(0f, .5f, .5f),
                    RGB = true,
                    Glow = true
                }
            }, {
                RankModel.Rank.S, new RankConfig {
                    Enable = true,
                    Name = "OK",
                    Color = new Color(1f, 1f, 1f),
                    Glow = true
                }
            }, {
                RankModel.Rank.A, new RankConfig {
                    Enable = true,
                    Name = "MHH",
                    Color = new Color(0f, 1f, 0f),
                    Glow = true
                }
            }, {
                RankModel.Rank.B, new RankConfig {
                    Enable = true,
                    Name = "DEAD",
                    Color = new Color(.5f, .5f, 0f),
                    Glow = true
                }
            }, {
                RankModel.Rank.C, new RankConfig {
                    Enable = true,
                    Name = "DEAD",
                    Color = new Color(1f, .5f, 0f),
                    Glow = true
                }
            }, {
                RankModel.Rank.D, new RankConfig {
                    Enable = true,
                    Name = "DEAD",
                    Color = new Color(1f, .3f, 0f),
                    Glow = true
                }
            }, {
                RankModel.Rank.E, new RankConfig {
                    Enable = true,
                    Name = "DEAD",
                    Color = new Color(1f, 0f, 0f),
                    Glow = true
                }
            }
        };

        public sealed class RankConfig : BaseOptions {

            public string Name { get; set; } = null!;
            public Color Color { get; set; }
            public bool RGB { get; set; }

        }

    }

}