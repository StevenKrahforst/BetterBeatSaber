using BetterBeatSaber.Utilities;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer; 

internal sealed class PlayersPlaceColorizer : IInitializable, ITickable {

    private RectangleFakeGlow? _rectangleFakeGlow;

    public void Initialize() =>
        _rectangleFakeGlow = GameObject.Find("PlayersPlace/RectangleFakeGlow").GetComponent<RectangleFakeGlow>();

    public void Tick() {
        if (_rectangleFakeGlow != null)
            _rectangleFakeGlow.color = RGB.Instance.FirstColor;
    }

}