using System;

using BetterBeatSaber.Providers;
using BetterBeatSaber.Utilities;

using HMUI;

using JetBrains.Annotations;

using Zenject;

namespace BetterBeatSaber.HudModifier; 

internal sealed class ComboHudModifier : HudModifier, IInitializable, ITickable, IDisposable {

    [UsedImplicitly]
    [Inject]
    private readonly ComboController _comboController = null!;
    
    [UsedImplicitly]
    [Inject]
    private readonly ComboUIController _comboUIController = null!;

    [UsedImplicitly]
    [Inject]
    private readonly MaterialProvider _materialProvider = null!;

    [UsedImplicitly]
    [Inject]
    private readonly BloomFontProvider _bloomFontProvider = null!;
    
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

        if (BetterBeatSaberConfig.Instance.ComboHudModifier.Glow) {
            _comboText.font = _bloomFontProvider.BloomFont;
            _comboNumText.font = _bloomFontProvider.BloomFont;
        }
        
        var fullComboLines = _comboUIController.GetComponentsInChildren<ImageView>();

        foreach (var fullComboLine in fullComboLines) {
            fullComboLine.gradient = true;
            if (BetterBeatSaberConfig.Instance.ComboHudModifier.Glow) {
                fullComboLine.material = _materialProvider.DistanceFieldMaterial;
            }
        }

        _topLine = fullComboLines[0];
        _bottomLine = fullComboLines[1];

        _comboController.comboBreakingEventHappenedEvent += OnComboBreakingEventHappenedEvent;
        
    }

    public void Tick() {
        
        if(_comboText != null)
            _comboText.color = RGB.Instance.FirstColor;
        
        if (_comboBroke)
            return;
        
        if(_comboNumText != null)
            _comboNumText.color = RGB.Instance.FirstColor;
        
        if (_topLine != null) {
            _topLine.color0 = RGB.Instance.FirstColor;
            _topLine.color1 = RGB.Instance.FirstColor;
        }
        
        // ReSharper disable once InvertIf
        if (_bottomLine != null) {
            _bottomLine.color0 = RGB.Instance.FirstColor;
            _bottomLine.color1 = RGB.Instance.FirstColor;
        }
        
    }
    
    public void Dispose() {
        _comboController.comboBreakingEventHappenedEvent -= OnComboBreakingEventHappenedEvent;
    }
    
    private void OnComboBreakingEventHappenedEvent() {
	    
        _comboBroke = true;
        
        if(_topLine != null) {
            _topLine.gameObject.SetActive(false);
        }
        
        if(_bottomLine != null) {
            _bottomLine.gameObject.SetActive(false);
        }
        
        if(_comboText != null) {
            _comboText.color = UnityEngine.Color.green;
        }
        
        _comboController.comboBreakingEventHappenedEvent -= OnComboBreakingEventHappenedEvent;
        
    }

}