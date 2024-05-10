using System;
using System.Linq;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.Interops;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class DustColorizer : IInitializable, IDisposable, ITickable {
    
    private bool _enabled;
    private ParticleSystem? _particleSystem;
    
    public void Initialize() {
        
        _enabled = BetterBeatSaberConfig.Instance.ColorizeDust.CurrentValue && !Tweaks55.Instance!.DisableCutParticles.CurrentValue;
        
        FetchDustParticleSystem();

        Tweaks55.Instance!.DisableGlobalParticles.OnValueChanged += OnDisableCutParticlesValueChanged;
        BetterBeatSaberConfig.Instance.ColorizeDust.OnValueChanged += OnColorizeDustValueChanged;

    }

    public void Tick() {
        if(_enabled && _particleSystem != null)
#pragma warning disable CS0618 // Type or member is obsolete
            _particleSystem.startColor = Manager.ColorManager.Instance.FirstColor.WithAlpha(_particleSystem.colorOverLifetime.color.Evaluate(_particleSystem.time).a);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public void Dispose() {
        Tweaks55.Instance!.DisableGlobalParticles.OnValueChanged -= OnDisableCutParticlesValueChanged;
        BetterBeatSaberConfig.Instance.ColorizeDust.OnValueChanged -= OnColorizeDustValueChanged;
    }
    
    private void OnDisableCutParticlesValueChanged(bool state) =>
        _enabled = !state && BetterBeatSaberConfig.Instance.ColorizeDust.CurrentValue;
    
    // TODO: On disable change back to default ig???
    private void OnColorizeDustValueChanged(bool state) =>
        _enabled = state && !Tweaks55.Instance!.DisableGlobalParticles;

    private void FetchDustParticleSystem() =>
        _particleSystem = Resources.FindObjectsOfTypeAll<ParticleSystem>().FirstOrDefault(particleSystem => particleSystem.name == "DustPS");

}