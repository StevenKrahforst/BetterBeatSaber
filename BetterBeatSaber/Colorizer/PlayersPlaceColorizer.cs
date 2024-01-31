using System;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer; 

// TODO: On disable change back to default
internal sealed class PlayersPlaceColorizer : IInitializable, IDisposable, ITickable {

    [UsedImplicitly]
    [Inject]
    private readonly Manager.ColorManager _colorManager = null!;
    
    private bool _enabled;
    private RectangleFakeGlow? _rectangleFakeGlow;

    public void Initialize() {

        _enabled = BetterBeatSaberConfig.Instance.ColorizePlayersPlace.CurrentValue;
        
        FetchRectangleFakeGlow();
        
        BetterBeatSaberConfig.Instance.ColorizePlayersPlace.OnValueChanged += OnColorizePlayersPlaceValueChanged;
        
    }

    public void Tick() {
        if (_enabled && _rectangleFakeGlow != null)
            _rectangleFakeGlow.color = _colorManager.FirstColor;
    }

    public void Dispose() =>
        BetterBeatSaberConfig.Instance.ColorizePlayersPlace.OnValueChanged -= OnColorizePlayersPlaceValueChanged;
    
    private void OnColorizePlayersPlaceValueChanged(bool state) {
        _enabled = true;
        if (!state && _rectangleFakeGlow != null)
            _rectangleFakeGlow.color = Color.white;
    }
    
    private void FetchRectangleFakeGlow() =>
        _rectangleFakeGlow = GameObject.Find("PlayersPlace/RectangleFakeGlow").GetComponent<RectangleFakeGlow>();

}