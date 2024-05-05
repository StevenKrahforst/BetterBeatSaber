using System.Collections.Generic;

using BetterBeatSaber.Extensions;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer;

internal sealed class MenuSignColorizer : IInitializable, ITickable {

    [UsedImplicitly]
    [Inject]
    private readonly MenuEnvironmentManager _menuEnvironmentManager = null!;
    
    private FlickeringNeonSign _flickeringNeonSign = null!;

    private SpriteRenderer? _eLogo;
    private SpriteRenderer _batLogo = null!;
    private SpriteRenderer _saberLogo = null!;
    private TubeBloomPrePassLight _bNeon = null!;
    private TubeBloomPrePassLight? _eNeon;
    private TubeBloomPrePassLight _aNeon = null!;
    private TubeBloomPrePassLight _tNeon = null!;
    private TubeBloomPrePassLight _saberNeon = null!;

    public void Initialize() {

        _flickeringNeonSign = _menuEnvironmentManager.transform.GetComponentInChildren<FlickeringNeonSign>();

        var parent = _flickeringNeonSign.transform.parent.gameObject;
        
        var renderers = parent.GetComponentsInChildren<SpriteRenderer>();
        var tubeLights = parent.GetComponentsInChildren<TubeBloomPrePassLight>();

        _eNeon = _flickeringNeonSign.GetField<TubeBloomPrePassLight, FlickeringNeonSign>("_light");
        _eLogo = _flickeringNeonSign.GetField<SpriteRenderer, FlickeringNeonSign>("_flickeringSprite");

        var rootRenderers = new List<SpriteRenderer>();
        var defaultEnvironment = _menuEnvironmentManager.transform.GetChild(0);

        for (var i = defaultEnvironment.childCount - 1; i >= 0; i--) {

            var child = defaultEnvironment.GetChild(i);
            var renderer = child.GetComponent<SpriteRenderer>();

            if (renderer != null)
                rootRenderers.Add(renderer);

            if (rootRenderers.Count >= 2)
                break;

        }

        rootRenderers[0].enabled = false;
        rootRenderers[1].enabled = false;

        foreach (var renderer in renderers) {
            switch (renderer.gameObject.name) {
                case "BatLogo":
                    _batLogo = renderer;
                    break;
                case "SaberLogo":
                    _saberLogo = renderer;
                    break;
            }
        }

        foreach (var light in tubeLights) {
            switch (light.gameObject.name) {
                case "BNeon":
                    _bNeon = light;
                    break;
                case "ANeon":
                    _aNeon = light;
                    break;
                case "TNeon":
                    _tNeon = light;
                    break;
                case "SaberNeon":
                    _saberNeon = light;
                    break;
            }
        }

        UpdateColors();

    }

    public void Tick() => UpdateColors();

    public void UpdateColors() {

        var color0 = Manager.ColorManager.Instance.FirstColor.WithAlpha(0.8f);
        var color1 = Manager.ColorManager.Instance.SecondColor.WithAlpha(0.8f);

        _batLogo.color = color0;
        if(_eLogo != null)
            _eLogo.color = color0;

        _flickeringNeonSign.SetField("_lightOnColor", color0);
        _flickeringNeonSign.SetField("_spriteOnColor", color0);

        var pss = _flickeringNeonSign.gameObject.GetComponentsInChildren<ParticleSystem>();

        foreach (var ps in pss) {
            var main = ps.main;
            main.startColor = color0;
        }

        _saberLogo.color = color1;

        var color2 = color0.WithAlpha(.7f);

        if(_eNeon != null)
            _eNeon.color = color2;
        
        _bNeon.color = color2;
        _aNeon.color = color2;
        _tNeon.color = color2;

        _saberNeon.color = color1.WithAlpha(.7f);

    }

}