﻿using System;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.Providers;

using HMUI;

using JetBrains.Annotations;

using Zenject;

namespace BetterBeatSaber.HudModifier; 

internal sealed class ComboHudModifier : IHudModifier, ITickable, IDisposable {

    [Inject, UsedImplicitly]
    private readonly ComboController _comboController = null!;
    
    [Inject, UsedImplicitly]
    private readonly ComboUIController _comboUIController = null!;

    [Inject, UsedImplicitly]
    private readonly MaterialProvider _materialProvider = null!;

    private CurvedTextMeshPro? _comboText;
    private CurvedTextMeshPro? _comboNumText;

    private ImageView? _topLine;
    private ImageView? _bottomLine;
    
    private bool _comboBroke;
    
    public void Initialize() {

        var comboTexts = _comboUIController.GetComponentsInChildren<CurvedTextMeshPro>();
        if (comboTexts is not { Length: 2 })
            return;

        _comboText = comboTexts[0];
        _comboNumText = comboTexts[1];
        
        var fullComboLines = _comboUIController.GetComponentsInChildren<ImageView>();
        foreach (var fullComboLine in fullComboLines) {
            fullComboLine.gradient = true;
            if (BetterBeatSaberConfig.Instance.ComboHudModifier.Glow)
                fullComboLine.material = _materialProvider.DistanceFieldMaterial;
        }

        _topLine = fullComboLines[0];
        _bottomLine = fullComboLines[1];
        
        if (BetterBeatSaberConfig.Instance.ComboHudModifier.Glow) {
            _comboText.font = TextMeshProExtensions.BloomFont;
            _comboNumText.font = TextMeshProExtensions.BloomFont;
        }

        _comboController.comboBreakingEventHappenedEvent += OnComboBreakingEventHappenedEvent;
        
    }

    public void Tick() {
        
        if(_comboText != null)
            _comboText.color = Manager.ColorManager.Instance.FirstColor;
        
        if (_comboBroke)
            return;
        
        if(_comboNumText != null)
            _comboNumText.color = Manager.ColorManager.Instance.FirstColor;
        
        if (_topLine != null) {
            _topLine.color0 = Manager.ColorManager.Instance.FirstColor;
            _topLine.color1 = Manager.ColorManager.Instance.FirstColor;
        }
        
        // ReSharper disable once InvertIf
        if (_bottomLine != null) {
            _bottomLine.color0 = Manager.ColorManager.Instance.FirstColor;
            _bottomLine.color1 = Manager.ColorManager.Instance.FirstColor;
        }
        
    }
    
    public void Dispose() =>
        _comboController.comboBreakingEventHappenedEvent -= OnComboBreakingEventHappenedEvent;
    
    private void OnComboBreakingEventHappenedEvent() {
	    
        _comboBroke = true;
        
        if(_topLine != null)
            _topLine.gameObject.SetActive(false);
        
        if(_bottomLine != null)
            _bottomLine.gameObject.SetActive(false);
        
        if(_comboText != null)
            _comboText.color = UnityEngine.Color.green;
        
        _comboController.comboBreakingEventHappenedEvent -= OnComboBreakingEventHappenedEvent;
 
    }

}