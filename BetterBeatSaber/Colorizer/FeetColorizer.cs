using BetterBeatSaber.Utilities;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer; 

internal sealed class FeetColorizer : IInitializable, ITickable {
    
    private SpriteRenderer? _renderer;

    public void Initialize() =>
        _renderer = GameObject.Find("PlayersPlace/Feet").GetComponent<SpriteRenderer>();

    public void Tick() {
        if (_renderer != null)
            _renderer.color = RGB.Instance.FirstColor;
    }

}