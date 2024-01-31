using System;
using System.Linq;

using BetterBeatSaber.Utilities;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer;

public sealed class GameColorizer : IInitializable, IDisposable, ITickable {

    private bool _colorizeDust;

    private ParticleSystem? _dustParticleSystem;
    
    public void Initialize() {

        _colorizeDust = BetterBeatSaberConfig.Instance.ColorizeDust && !BetterBeatSaberConfig.Instance.DisableDust;
        FetchDustParticleSystem();
        
        BetterBeatSaberConfig.Instance.ColorizeDust.OnValueChanged += OnColorizeDustValueChanged;
        BetterBeatSaberConfig.Instance.DisableDust.OnValueChanged += OnDisableDustValueChanged;

    }

    public void Tick() {
        
        if(_colorizeDust && _dustParticleSystem != null)
#pragma warning disable CS0618 // Type or member is obsolete
            _dustParticleSystem.startColor = RGB.Instance.FirstColor;
#pragma warning restore CS0618 // Type or member is obsolete
        
        
        
    }
    
    public void Dispose() {
        
        
        
    }

    #region Observable Handlers
    
    private void OnColorizeDustValueChanged(bool state) {
        if(state)
            FetchDustParticleSystem();
        _colorizeDust = state && !BetterBeatSaberConfig.Instance.DisableDust;
    }

    private void OnDisableDustValueChanged(bool state) {
        if(!state)
            FetchDustParticleSystem();
        _colorizeDust = !state && BetterBeatSaberConfig.Instance.ColorizeDust;
    }
    
    #endregion

    #region Utilities

    private void FetchDustParticleSystem() =>
        _dustParticleSystem = Resources.FindObjectsOfTypeAll<ParticleSystem>().FirstOrDefault(particleSystem => particleSystem.name == "DustPS");

    #endregion

}