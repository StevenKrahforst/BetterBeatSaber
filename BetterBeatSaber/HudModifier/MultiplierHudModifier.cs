using System;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.Providers;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace BetterBeatSaber.HudModifier; 

internal sealed class MultiplierHudModifier : IHudModifier, ITickable, IDisposable {

    [UsedImplicitly]
    [Inject]
    private readonly ScoreMultiplierUIController _scoreMultiplierUIController = null!;

    [UsedImplicitly]
    [Inject]
    private readonly IScoreController _scoreController = null!;

    [UsedImplicitly]
    [Inject]
    private readonly MaterialProvider _materialProvider = null!;
    
    private int _currentMultiplier;

    private Image _background = null!;
    private Image _foreground = null!;

    public void Initialize() {
        
        _scoreController.multiplierDidChangeEvent += HandleMultiplierDidChange;
        
        _background = _scoreMultiplierUIController.transform.Find("BGCircle").GetComponent<Image>();
        _foreground = _scoreMultiplierUIController.transform.Find("FGCircle").GetComponent<Image>();

        if (BetterBeatSaberConfig.Instance.MultiplierHudModifier.Glow) {
            _background.material = _materialProvider.DefaultUIMaterial;
            _foreground.material = _materialProvider.DefaultUIMaterial;
        }

        HandleMultiplierDidChange(1, 0f);
        
    }

    public void Tick() {
        
        if (!_scoreMultiplierUIController.isActiveAndEnabled)
            return;

        switch (_currentMultiplier) {
            case 1:
                _background.color = Color.red.LerpHSV(Color.yellow, _foreground.fillAmount).WithAlpha(.25f);
                _foreground.color = Color.red.LerpHSV(Color.yellow, _foreground.fillAmount);
                break;
            case 2:
                _background.color = Color.yellow.LerpHSV(Color.green, _foreground.fillAmount).WithAlpha(.25f);
                _foreground.color = Color.yellow.LerpHSV(Color.green, _foreground.fillAmount);
                break;
            case 4:
                _background.color = Color.green.LerpHSV(Manager.ColorManager.Instance.FirstColor, _foreground.fillAmount).WithAlpha(.25f);
                _foreground.color = Color.green.LerpHSV(Manager.ColorManager.Instance.FirstColor, _foreground.fillAmount);
                break;
            case 8:
                _background.color = Manager.ColorManager.Instance.FirstColor;
                break;
        }
        
    }

    public void Dispose() {
        _scoreController.multiplierDidChangeEvent -= HandleMultiplierDidChange;
    }
    
    private void HandleMultiplierDidChange(int multiplier, float progress) {
        _currentMultiplier = multiplier;
    }
    
}