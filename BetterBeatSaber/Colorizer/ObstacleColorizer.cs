using System.Reflection;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.Utilities;

using UnityEngine;

namespace BetterBeatSaber.Colorizer; 

internal sealed class ObstacleColorizer : MonoBehaviour {

    private static FieldInfo? _obstacleFrameField;
    private static FieldInfo? _obstacleFakeGlowField;
    private static FieldInfo? _addColorMultiplierField;
    private static FieldInfo? _obstacleCoreLerpToWhiteFactorField;
    private static FieldInfo? _materialPropertyBlockControllersField;
    
    private static readonly int TintColorID = Shader.PropertyToID("_TintColor");
    private static readonly int AddColorID = Shader.PropertyToID("_AddColor");

    private ParametricBoxFrameController? _obstacleFrame;
    private ParametricBoxFakeGlowController? _obstacleFakeGlow;
    
    private float _addColorMultiplier;
    private float _obstacleCoreLerpToWhiteFactor;
    
    private MaterialPropertyBlockController[]? _materialPropertyBlockControllers = null!;

    private void Start() {

        // Get Fields
        _obstacleFrameField ??= typeof(StretchableObstacle).GetField("_obstacleFrame", BindingFlags.Instance | BindingFlags.NonPublic);
        _obstacleFakeGlowField ??= typeof(StretchableObstacle).GetField("_obstacleFakeGlow", BindingFlags.Instance | BindingFlags.NonPublic);

        _addColorMultiplierField ??= typeof(StretchableObstacle).GetField("_addColorMultiplier", BindingFlags.Instance | BindingFlags.NonPublic);
        _obstacleCoreLerpToWhiteFactorField ??= typeof(StretchableObstacle).GetField("_obstacleCoreLerpToWhiteFactor", BindingFlags.Instance | BindingFlags.NonPublic);
        
        _materialPropertyBlockControllersField ??= typeof(StretchableObstacle).GetField("_materialPropertyBlockControllers", BindingFlags.Instance | BindingFlags.NonPublic);
        
        // Rest
        var stretchableObstacle = gameObject.GetComponent<ObstacleController>().GetComponent<StretchableObstacle>();
        
        _obstacleFrame = (ParametricBoxFrameController?) _obstacleFrameField?.GetValue(stretchableObstacle);
        _obstacleFakeGlow = (ParametricBoxFakeGlowController?) _obstacleFakeGlowField?.GetValue(stretchableObstacle);

        _addColorMultiplier = (float?) _addColorMultiplierField?.GetValue(stretchableObstacle) ?? .1f;
        _obstacleCoreLerpToWhiteFactor = (float?) _obstacleCoreLerpToWhiteFactorField?.GetValue(stretchableObstacle) ?? .75f;
        
        _materialPropertyBlockControllers = (MaterialPropertyBlockController[]?) _materialPropertyBlockControllersField?.GetValue(stretchableObstacle);
        
    }

    private void Update() {
        
        var color = RGB.Instance.FirstColor;

        if (_obstacleFrame != null) {
            _obstacleFrame.color = color;
            _obstacleFrame.Refresh();
        }
        
        if (_obstacleFakeGlow != null) {
            _obstacleFakeGlow.color = color;
            _obstacleFakeGlow.Refresh();
        }

        var value = (color * _addColorMultiplier).WithAlpha(0f);

        if (_materialPropertyBlockControllers == null)
            return;
        
        foreach (var materialPropertyBlockController in _materialPropertyBlockControllers) {
            materialPropertyBlockController.materialPropertyBlock.SetColor(AddColorID, value);
            materialPropertyBlockController.materialPropertyBlock.SetColor(TintColorID, Color.Lerp(color, Color.white, _obstacleCoreLerpToWhiteFactor));
            materialPropertyBlockController.ApplyChanges();
        }
        
    }
    
}