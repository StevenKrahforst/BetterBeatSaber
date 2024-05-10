using System;
using System.Collections;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;

namespace BetterBeatSaber.Manager;

public sealed class ColorManager : Utilities.PersistentSingleton<ColorManager> {

    private float _duration;
    
    public float FirstColorHue { get; private set; }
    public float SecondColorHue { get; private set; }
    public float ThirdColorHue { get; private set; }
    
    public Color FirstColor { get; private set; }
    public Color SecondColor { get; private set; }
    public Color ThirdColor { get; private set; }

    private void Start() {
        
        _duration = BetterBeatSaberConfig.Instance.ColorUpdateDurationTime.CurrentValue;
        
        BetterBeatSaberConfig.Instance.ColorUpdateDurationTime.OnValueChanged += OnColorUpdateDurationTimeValueChanged;

        if(BetterBeatSaberConfig.Instance.SignalRGBIntegration)
            StartCoroutine(UpdateIntegrations());
        
    }

    protected override void OnDestroy() {
        StopCoroutine(UpdateIntegrations());
        BetterBeatSaberConfig.Instance.ColorUpdateDurationTime.OnValueChanged += OnColorUpdateDurationTimeValueChanged;
        base.OnDestroy();
    }

    private void OnColorUpdateDurationTimeValueChanged(float duration) =>
        _duration = duration;
    
    private void Update() {
        
        var time = Time.time / _duration;
        var hue = Mathf.Clamp(time % 2f >= 1f ? 1f - time % 1f : time % 1f, .05f, .9f);

        FirstColorHue = hue;
        SecondColorHue = Mathf.Max(1f, hue + .05f);
        ThirdColorHue = Mathf.Max(1f, hue + .15f);
        
        FirstColor = Color.HSVToRGB(FirstColorHue, 1f, 1f);
        SecondColor = Color.HSVToRGB(SecondColorHue, 1f, 1f);
        ThirdColor = Color.HSVToRGB(ThirdColorHue, 1f, 1f);
        
    }

    #region Integrations

    private IEnumerator UpdateIntegrations() {
        for(;;) {
            if(BetterBeatSaberConfig.Instance.SignalRGBIntegration)
                yield return SetSignalRGBHue((int) Math.Floor(FirstColorHue * 360));
            yield return new WaitForSeconds(BetterBeatSaberConfig.Instance.RGBIntegrationUpdateInterval.CurrentValue);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private static IEnumerator SetSignalRGBHue(int hue) {
        var request = new UnityWebRequest($"http://localhost:16034/canvas/event?sender=bbs&event={hue}");
        request.method = "POST";
        yield return request.SendWebRequest();
    }

    #endregion

}