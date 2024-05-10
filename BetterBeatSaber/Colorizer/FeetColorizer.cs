using System;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer; 

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class FeetColorizer : IInitializable, IDisposable, ITickable {

    private SpriteRenderer? _renderer;

    public void Initialize() {
        _renderer = GameObject.Find("PlayersPlace/Feet").GetComponent<SpriteRenderer>();
        BetterBeatSaberConfig.Instance.ColorizeFeet.OnValueChanged += OnColorizeFeetValueChanged;
    }

    public void Tick() {
        if (BetterBeatSaberConfig.Instance.ColorizeFeet.CurrentValue && _renderer != null)
            _renderer.color = Manager.ColorManager.Instance.FirstColor;
    }
    
    public void Dispose() =>
        BetterBeatSaberConfig.Instance.ColorizeFeet.OnValueChanged -= OnColorizeFeetValueChanged;
    
    private void OnColorizeFeetValueChanged(bool state) {
        if(!state && _renderer != null)
            _renderer.color = Color.white;
    }
    
}