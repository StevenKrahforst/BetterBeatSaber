using System.Collections.Generic;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.Utilities;

using JetBrains.Annotations;

using TMPro;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Colorizer;

public sealed class MenuSignColorizer : IInitializable, ITickable {

    [UsedImplicitly]
    [Inject]
    private readonly MenuEnvironmentManager _menuEnvironmentManager = null!;

    private FlickeringNeonSign _flickeringNeonSign = null!;

    //private TextMeshProUGUI? _betterText;

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
        
        //_betterText = CreateBetterText(parent.transform);

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

    private TextMeshProUGUI CreateBetterText(Transform parent) {

        parent.gameObject.AddComponent<Canvas>();

        var obj = new GameObject("CustomUIText");
        Object.DontDestroyOnLoad(obj);

        obj.transform.SetParent(parent);

        var text = obj.AddComponent<TextMeshProUGUI>();

        text.rectTransform.SetParent(_flickeringNeonSign.transform.parent, false);

        text.rectTransform.anchorMin = new Vector2(.5f, .5f);
        text.rectTransform.anchorMax = new Vector2(.5f, .5f);

        text.rectTransform.sizeDelta = new Vector2(20f, 5f);
        text.rectTransform.anchoredPosition = new Vector2(0f, 3.25f);

        //text.font = AssetProvider.Instance!.DefaultFontBloom;
        text.text = "Better";
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        text.fontStyle |= FontStyles.Italic;
        text.fontSize = 6f;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;

        return text;

    }

    public void UpdateColors() {

        var color0 = RGB.Instance.FirstColor.WithAlpha(0.8f);
        var color1 = RGB.Instance.SecondColor.WithAlpha(0.8f);

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

        #region Better Text

        /*if (_betterText == null)
            return;
        
        _betterText.ForceMeshUpdate();

        var steps = Steps(_betterText.textInfo.characterCount);
        var gradients = new VertexGradient[_betterText.textInfo.characterCount];

        for (var index = 0; index < _betterText.textInfo.characterCount; index++) {

            gradients[index] = new VertexGradient(steps[index], steps[index + 1], steps[index], steps[index + 1]);

            var characterInfo = _betterText.textInfo.characterInfo[index];

            if (!characterInfo.isVisible || characterInfo.character == ' ')
                continue;

            var colors = _betterText.textInfo.meshInfo[characterInfo.materialReferenceIndex].colors32;
            var vertexIndex = _betterText.textInfo.characterInfo[index].vertexIndex;

            colors[vertexIndex + 0] = gradients[index].bottomLeft;
            colors[vertexIndex + 1] = gradients[index].topLeft;
            colors[vertexIndex + 2] = gradients[index].bottomRight;
            colors[vertexIndex + 3] = gradients[index].topRight;

        }

        _betterText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);*/

        #endregion

    }

    private static Color[] Steps(int stepsAmount) {

        stepsAmount += 2;

        var start = RGB.Instance.FirstColor.WithAlpha(.75f);
        var end = RGB.Instance.SecondColor.WithAlpha(.75f);

        var result = new Color[stepsAmount];
        var r = (end.r - start.r) / (stepsAmount - 1);
        var g = (end.g - start.g) / (stepsAmount - 1);
        var b = (end.b - start.b) / (stepsAmount - 1);
        var a = (end.a - start.a) / (stepsAmount - 1);

        for (var index = 0; index < stepsAmount; index++)
            result[index] = new Color(start.r + r * index, start.g + g * index, start.b + b * index, start.a + a * index);

        return result;
    }

}