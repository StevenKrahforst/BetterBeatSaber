using System;
using System.Linq;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer; 

internal sealed class PlayersPlaceColorizer : IInitializable, IDisposable, ITickable {

    public const string GameObjectName = "PlayersPlace";
    
    [UsedImplicitly]
    [Inject]
    private readonly Manager.ColorManager _colorManager = null!;
    
    private bool _enabled;
    private RectangleFakeGlow? _rectangleFakeGlow;

    public void Initialize() {

        FetchRectangleFakeGlow();
        
        BetterBeatSaberConfig.Instance.ColorizePlayersPlace.OnValueChanged += OnColorizePlayersPlaceValueChanged;

        OnColorizePlayersPlaceValueChanged(BetterBeatSaberConfig.Instance.ColorizePlayersPlace.CurrentValue);

    }

    public void Tick() {
        if (_enabled && _rectangleFakeGlow != null)
            _rectangleFakeGlow.color = _colorManager.FirstColor;
    }

    public void Dispose() =>
        BetterBeatSaberConfig.Instance.ColorizePlayersPlace.OnValueChanged -= OnColorizePlayersPlaceValueChanged;
    
    private void OnColorizePlayersPlaceValueChanged(bool state) {
        _enabled = state && BetterBeatSaberConfig.Instance.IgnoredLevelGameObjects.Contains(GameObjectName);
        if (!state && _rectangleFakeGlow != null)
            _rectangleFakeGlow.color = Color.white;
    }
    
    private void FetchRectangleFakeGlow() =>
        _rectangleFakeGlow = GameObject.Find("PlayersPlace/RectangleFakeGlow")?.GetComponent<RectangleFakeGlow>();

}