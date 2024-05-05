using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.Providers;

using HMUI;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

using Random = UnityEngine.Random;

namespace BetterBeatSaber.HudModifier;

internal sealed class EnergyHudModifier : HudModifier, IInitializable, ITickable, IDisposable {

    [UsedImplicitly]
    [Inject]
    private readonly IGameEnergyCounter _energyCounter = null!;
    
    [UsedImplicitly]
    [Inject]
    private readonly GameEnergyUIPanel _gameEnergyUIPanel = null!;
    
    [UsedImplicitly]
    [Inject]
    private readonly ComboController _comboController = null!;
    
    [UsedImplicitly]
    [Inject]
    private readonly GameplayModifiers _gameplayModifiers = null!;
    
    [UsedImplicitly]
    [Inject]
    private readonly MaterialProvider _materialProvider = null!;

    private Image? _energyBar;

    #region Shaking
    
    private Vector3 _originalPosition;
    
    private bool _shaking;
    private float _shakingIntensity;
    private float _shakingDuration;
    private bool _isDead;

    #endregion

    private FieldInfo? _batteryLifeSegmentsField;
    
    public void Initialize() {
        Utilities.SharedCoroutineStarter.Instance.StartCoroutine(PrepareColorsForEnergyType(_gameplayModifiers.energyType));
        _batteryLifeSegmentsField = typeof(GameEnergyUIPanel).GetField("_batteryLifeSegments", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public void Tick() {
        
        if (_energyBar == null)
            return;

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_gameplayModifiers.instaFail || (_energyCounter.energy == 1f && _gameplayModifiers.energyType == GameplayModifiers.EnergyType.Bar)) {
            _energyBar.color = Manager.ColorManager.Instance.FirstColor;
        }
        
    }
    
    public void Dispose() {
        _energyCounter.gameEnergyDidChangeEvent -= OnGameEnergyDidChangeEvent;
        if(BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeOnComboBreak)
            _comboController.comboBreakingEventHappenedEvent -= OnComboBreakingEventHappenedEvent;
    }
    
    private IEnumerator PrepareColorsForEnergyType(GameplayModifiers.EnergyType type) {
        
        yield return new WaitUntil(() => _gameEnergyUIPanel != null);
        
        if (type == GameplayModifiers.EnergyType.Battery) {

            var field = (List<Image>?) _batteryLifeSegmentsField?.GetValue(_gameEnergyUIPanel);
            if(field == null)
                yield break;
            
            field[0].color = Color.red;
            field[1].color = Color.red.LerpHSV(Color.yellow, .34f);
            field[2].color = Color.yellow.LerpHSV(Color.green, .66f);
            //field[1].color = HSBColor.Lerp(HSBColor.FromColor(Color.red), HSBColor.FromColor(Color.yellow), .34f).ToColor();
            //field[2].color = HSBColor.Lerp(HSBColor.FromColor(Color.yellow), HSBColor.FromColor(Color.green), .66f).ToColor();
            field[3].color = Color.green;

            if (!BetterBeatSaberConfig.Instance.EnergyHudModifier.Glow)
                yield break;
            
            foreach (var f in field)
                f.material = _materialProvider.DistanceFieldMaterial;

            yield break;
        }

        if (type == GameplayModifiers.EnergyType.Bar) {
            if (_gameplayModifiers.instaFail) {
                _energyBar = _gameEnergyUIPanel.transform.Find("BatteryLifeSegment(Clone)").GetComponent<ImageView>();
                if (BetterBeatSaberConfig.Instance.EnergyHudModifier.Glow) {
                    _energyBar.material = _materialProvider.DistanceFieldMaterial;
                }
                _energyBar.color = Color.green;
            } else {
                _energyBar = _gameEnergyUIPanel.transform.Find("EnergyBarWrapper/EnergyBar").GetComponent<ImageView>();
                if (BetterBeatSaberConfig.Instance.EnergyHudModifier.Glow) {
                    _energyBar.material = _materialProvider.DistanceFieldMaterial;
                }
                _energyBar.color = Color.yellow;
            }
        }

        _originalPosition = _gameEnergyUIPanel.transform.position;
        
        _energyCounter.gameEnergyDidChangeEvent += OnGameEnergyDidChangeEvent;
        
        if(BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeOnComboBreak)
            _comboController.comboBreakingEventHappenedEvent += OnComboBreakingEventHappenedEvent;

    }
    
    private IEnumerator Shake() {
        var elapsed = 0f;
        while (elapsed < _shakingDuration) {
            var newPosition = Random.insideUnitSphere * (Time.deltaTime * _shakingIntensity);
            var position = _gameEnergyUIPanel.transform.position;
            newPosition.z = position.z;
            //newPosition.y = position.y - Random.Range(-.5f, .5f);
            newPosition.y = position.y - Random.Range(-.025f, .025f);
            _gameEnergyUIPanel.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return 0;
        }
        _shaking = false;
        _gameEnergyUIPanel.transform.position = _originalPosition;
    }

    private void OnComboBreakingEventHappenedEvent() {
        
        if (_shaking) {
            _shakingIntensity += BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeIntensity;
            _shakingDuration += BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeDuration;
            return;
        }
        
        _shaking = true;
        _shakingIntensity = BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeStartIntensity;
        _shakingDuration = BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeStartDuration;
        
        Utilities.SharedCoroutineStarter.Instance.StartCoroutine(Shake());
        
    }
    
    private void OnGameEnergyDidChangeEvent(float energy) {
        
        if (_energyBar == null)
            return;
        
        _energyBar.color = energy switch {
            .5f => Color.yellow,
            > .5f => Color.yellow.LerpHSV(Color.green, (energy - .5f) * 2f),
            < .5f => Color.red.LerpHSV(Color.yellow, energy * 2f),
            _ => _energyBar.color
        };
        
        if (energy > 0f || !BetterBeatSaberConfig.Instance.EnergyHudModifier.ShakeOnComboBreak || _isDead)
            return;
        
        _comboController.comboBreakingEventHappenedEvent -= OnComboBreakingEventHappenedEvent;
        
        _isDead = true;
        
    }
    
    public class Options : BaseOptions {

        public bool ShakeOnComboBreak { get; set; } = true;
        
        public float ShakeStartIntensity { get; set; } = 10f;
        public float ShakeIntensity { get; set; } = 2.5f;

        public float ShakeStartDuration { get; set; } = .25f;
        public float ShakeDuration { get; set; } = .2f;
        
    }

}