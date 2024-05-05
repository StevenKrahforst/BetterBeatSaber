using System;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer; 

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class FeetColorizer : IInitializable, IDisposable, ITickable {

    private bool _enabled;
    private SpriteRenderer? _renderer;

    public void Initialize() {

        _enabled = BetterBeatSaberConfig.Instance.ColorizeFeet.CurrentValue;

        FetchRenderer();
        
        BetterBeatSaberConfig.Instance.ColorizeFeet.OnValueChanged += OnColorizeFeetValueChanged;
        
    }

    public void Tick() {
        if (_enabled && _renderer != null)
            _renderer.color = Manager.ColorManager.Instance.FirstColor;
    }
    
    public void Dispose() =>
        BetterBeatSaberConfig.Instance.ColorizeFeet.OnValueChanged -= OnColorizeFeetValueChanged;
    
    private void OnColorizeFeetValueChanged(bool state) {
        _enabled = state;
        if(!state && _renderer != null)
            _renderer.color = Color.white;
    }
    
    private void FetchRenderer() =>
        _renderer = GameObject.Find("PlayersPlace/Feet").GetComponent<SpriteRenderer>();

}