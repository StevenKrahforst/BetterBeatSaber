using System;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Manager;

public sealed class ColorManager : IInitializable, IDisposable, ITickable {

    public static ColorManager? Instance { get; private set; }
    
    private float _duration;
    
    public float FirstColorHue { get; private set; }
    public float SecondColorHue { get; private set; }
    public float ThirdColorHue { get; private set; }
    
    public Color FirstColor { get; private set; }
    public Color SecondColor { get; private set; }
    public Color ThirdColor { get; private set; }

    public ColorManager() {
        Instance = this;
    }

    public void Initialize() {
        _duration = BetterBeatSaberConfig.Instance.ColorUpdateDurationTime.CurrentValue;
        BetterBeatSaberConfig.Instance.ColorUpdateDurationTime.OnValueChanged += OnColorUpdateDurationTimeValueChanged;
    }

    public void Dispose() =>
        BetterBeatSaberConfig.Instance.ColorUpdateDurationTime.OnValueChanged += OnColorUpdateDurationTimeValueChanged;
    
    private void OnColorUpdateDurationTimeValueChanged(float duration) =>
        _duration = duration;

    public void Tick() {
        
        var time = Time.time / _duration;
        var hue = Mathf.Clamp(time % 2f >= 1f ? 1f - time % 1f : time % 1f, .05f, .9f);

        FirstColorHue = hue;
        SecondColorHue = Mathf.Max(1f, hue + .05f);
        ThirdColorHue = Mathf.Max(1f, hue + .15f);
        
        FirstColor = Color.HSVToRGB(FirstColorHue, 1f, 1f);
        SecondColor = Color.HSVToRGB(SecondColorHue, 1f, 1f);
        ThirdColor = Color.HSVToRGB(ThirdColorHue, 1f, 1f);
        
    }

}