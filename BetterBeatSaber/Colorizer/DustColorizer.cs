using System;
using System.Linq;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer;

// TODO: On disable change back to default
public sealed class DustColorizer : IInitializable, IDisposable, ITickable {

    [UsedImplicitly]
    [Inject]
    private readonly Manager.ColorManager _colorManager = null!;
    
    private bool _enabled;
    private ParticleSystem? _particleSystem;
    
    public void Initialize() {
        
        _enabled = BetterBeatSaberConfig.Instance.ColorizeDust && !BetterBeatSaberConfig.Instance.DisableDust;
        
        FetchDustParticleSystem();
        
        BetterBeatSaberConfig.Instance.ColorizeDust.OnValueChanged += OnColorizeDustValueChanged;
        BetterBeatSaberConfig.Instance.DisableDust.OnValueChanged += OnDisableDustValueChanged;

    }

    public void Tick() {
        if(_enabled && _particleSystem != null)
#pragma warning disable CS0618 // Type or member is obsolete
            _particleSystem.startColor = _colorManager.FirstColor.ColorWithAlpha(_particleSystem.colorOverLifetime.color.Evaluate(_particleSystem.time).a);
#pragma warning restore CS0618 // Type or member is obsolete
    }
    
    public void Dispose() {
        BetterBeatSaberConfig.Instance.ColorizeDust.OnValueChanged -= OnColorizeDustValueChanged;
        BetterBeatSaberConfig.Instance.DisableDust.OnValueChanged -= OnDisableDustValueChanged;
    }

    private void OnColorizeDustValueChanged(bool state) {
        _enabled = state && !BetterBeatSaberConfig.Instance.DisableDust;
    }

    private void OnDisableDustValueChanged(bool state) {
        _enabled = !state && BetterBeatSaberConfig.Instance.ColorizeDust;
    }
    
    private void FetchDustParticleSystem() =>
        _particleSystem = Resources.FindObjectsOfTypeAll<ParticleSystem>().FirstOrDefault(particleSystem => particleSystem.name == "DustPS");

}