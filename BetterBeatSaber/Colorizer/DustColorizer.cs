using BetterBeatSaber.Utilities;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer; 

internal sealed class DustColorizer : IInitializable, ITickable {
    
    private ParticleSystem? _particleSystem;

    public void Initialize() {
        _particleSystem = GameObject.Find("DustPS")?.GetComponent<ParticleSystem>();
        if(_particleSystem == null)
            BetterBeatSaber.Instance.Logger.Warn("No Dust Particles found, skipping colorization, ig Kinsi doesn't like dust ...");
    }

    public void Tick() {
        if (_particleSystem != null)
#pragma warning disable CS0618 // Type or member is obsolete
            _particleSystem.startColor = RGB.Instance.FirstColor;
#pragma warning restore CS0618 // Type or member is obsolete
    }

}