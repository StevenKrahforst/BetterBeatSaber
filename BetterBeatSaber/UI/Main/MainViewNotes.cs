using System;
using System.Linq;

using BetterBeatSaber.Enums;
using BetterBeatSaber.Extensions;
using BetterBeatSaber.Utilities;

using IPA.Utilities;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace BetterBeatSaber.UI.Main;

// TODO: Add Custom Notes Support
public partial class MainView {

    private static readonly FieldAccessor<MenuTransitionsHelper, StandardLevelScenesTransitionSetupDataSO>.Accessor StandardLevelScenesTransitionSetupDataAccessor = FieldAccessor<MenuTransitionsHelper, StandardLevelScenesTransitionSetupDataSO>.GetAccessor("_standardLevelScenesTransitionSetupData");
    
    private static readonly FieldAccessor<StandardLevelScenesTransitionSetupDataSO, SceneInfo>.Accessor StandardGameplaySceneInfoAccessor = FieldAccessor<StandardLevelScenesTransitionSetupDataSO, SceneInfo>.GetAccessor("_standardGameplaySceneInfo");
    private static readonly FieldAccessor<StandardLevelScenesTransitionSetupDataSO, SceneInfo>.Accessor GameCoreSceneInfoAccessor = FieldAccessor<StandardLevelScenesTransitionSetupDataSO, SceneInfo>.GetAccessor("_gameCoreSceneInfo");
    
    private static readonly FieldAccessor<BeatmapObjectsInstaller, GameNoteController>.Accessor NormalBasicNotePrefabAccessor = FieldAccessor<BeatmapObjectsInstaller, GameNoteController>.GetAccessor("_normalBasicNotePrefab");
    private static readonly FieldAccessor<BeatmapObjectsInstaller, BombNoteController>.Accessor BombNotePrefabAccessor = FieldAccessor<BeatmapObjectsInstaller, BombNoteController>.GetAccessor("_bombNotePrefab");

    private GameObject? _noteTemplate;
    private GameObject? _bombTemplate;

    private GameObject? _noteLeft;
    private Outline? _noteLeftOutline;

    private GameObject? _noteRight;
    private Outline? _noteRightOutline;
    
    private void GetTemplates() {
        
        var menuTransitionsHelper = Resources.FindObjectsOfTypeAll<MenuTransitionsHelper>().FirstOrDefault();
        if (menuTransitionsHelper == null)
            throw new Exception("NULLE");

        var standardLevelScenesTransitionSetupData = StandardLevelScenesTransitionSetupDataAccessor(ref menuTransitionsHelper);

        var standardGameplaySceneName = StandardGameplaySceneInfoAccessor(ref standardLevelScenesTransitionSetupData).sceneName;
        var gameCoreSceneName = GameCoreSceneInfoAccessor(ref standardLevelScenesTransitionSetupData).sceneName;
        
        if(standardGameplaySceneName == null || gameCoreSceneName == null)
            throw new Exception("NULLEEEEEEEEE");
        
        SceneManager.LoadSceneAsync(standardGameplaySceneName, LoadSceneMode.Additive).completed += _ => {
            SceneManager.LoadSceneAsync(gameCoreSceneName, LoadSceneMode.Additive).completed += _ => {
                
                var beatmapObjectsInstaller = Resources.FindObjectsOfTypeAll<BeatmapObjectsInstaller>().FirstOrDefault();
                
                var originalNotePrefab = NormalBasicNotePrefabAccessor(ref beatmapObjectsInstaller);
                _noteTemplate = Instantiate(originalNotePrefab.transform.GetChild(0).gameObject);
                _noteTemplate.gameObject.SetActive(false);

                DontDestroyOnLoad(_noteTemplate);

                var originalBombPrefab = BombNotePrefabAccessor(ref beatmapObjectsInstaller);
                _bombTemplate = Instantiate(originalBombPrefab.transform.GetChild(0).gameObject);
                _bombTemplate.gameObject.SetActive(false);

                DontDestroyOnLoad(_bombTemplate);

                SceneManager.UnloadSceneAsync(standardGameplaySceneName);
                SceneManager.UnloadSceneAsync(gameCoreSceneName);
                
            };
        };
        
    }

    private void CreateNotes() {

        var playerData = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First().playerData;
        if (playerData == null)
            return;
        
        if (_noteTemplate == null)
            return;
        
        _noteLeft = Instantiate(_noteTemplate);
        _noteRight = Instantiate(_noteTemplate);

        //_note.transform.SetParent(gameObject.transform.parent.transform, false);

        _noteLeft.name = "Left Note Preview";
        _noteRight.name = "Right Note Preview";
        
        _noteLeft.transform.localPosition = new Vector3(2f, 1.5f, 4f);
        _noteRight.transform.localPosition = new Vector3(-2f, 1.5f, 4f);
        
        _noteLeft.transform.localRotation = Quaternion.Euler(0, 0, 0);
        _noteRight.transform.localRotation = Quaternion.Euler(0, 200, 0);
        
        _noteLeft.transform.localScale = BetterBeatSaberConfig.Instance.NoteSize.CurrentValue * Vector3.one;
        _noteRight.transform.localScale = BetterBeatSaberConfig.Instance.NoteSize.CurrentValue * Vector3.one;

        foreach (Transform child in _noteLeft.transform)
            child.gameObject.SetActive(true);
        
        foreach (Transform child in _noteRight.transform)
            child.gameObject.SetActive(true);

        var colorScheme = playerData.colorSchemesSettings.GetOverrideColorScheme();
        
        var leftNoteRenderer = _noteLeft.GetComponent<MeshRenderer>();
        if (leftNoteRenderer != null && leftNoteRenderer) {
            foreach (var materialPropertyBlockController in _noteLeft.GetComponents<MaterialPropertyBlockController>()) {
                materialPropertyBlockController.materialPropertyBlock.SetColor(Shader.PropertyToID("_Color"), colorScheme.saberAColor.WithAlpha(1f));
                materialPropertyBlockController.ApplyChanges();
            }
            leftNoteRenderer.receiveShadows = false;
            leftNoteRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }
        
        var rightNoteRenderer = _noteRight.GetComponent<MeshRenderer>();
        if (rightNoteRenderer != null && rightNoteRenderer) {
            foreach (var materialPropertyBlockController in _noteRight.GetComponents<MaterialPropertyBlockController>()) {
                materialPropertyBlockController.materialPropertyBlock.SetColor(Shader.PropertyToID("_Color"), colorScheme.saberBColor.WithAlpha(1f));
                materialPropertyBlockController.ApplyChanges();
            }
            rightNoteRenderer.receiveShadows = false;
            rightNoteRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }
        
        _noteLeftOutline = _noteLeft.AddComponent<Outline>();
        _noteRightOutline = _noteLeft.AddComponent<Outline>();

        _noteLeftOutline.enabled = NoteOutlinesEnable;
        _noteLeftOutline.OutlineWidth = NoteOutlinesWidth;
        _noteLeftOutline.Visibility = NoteOutlinesVisibility;
        _noteLeftOutline.Glowing = NoteOutlinesGlow;

        _noteRightOutline.enabled = NoteOutlinesEnable;
        _noteRightOutline.OutlineWidth = NoteOutlinesWidth;
        _noteRightOutline.Visibility = NoteOutlinesVisibility;
        _noteRightOutline.Glowing = NoteOutlinesGlow;
        
        _noteLeft.SetActive(true);
        _noteRight.SetActive(true);

    }

    private void DestroyNotes() {
        
        Destroy(_noteLeft);
        Destroy(_noteLeftOutline);
        
        Destroy(_noteRight);
        Destroy(_noteRightOutline);
        
    }

    #region Config

    public float NoteSize {
        get => BetterBeatSaberConfig.Instance.NoteSize.CurrentValue * 100;
        set {
            var scale = value / 100;
            BetterBeatSaberConfig.Instance.NoteSize.SetValue(scale);
            if (_noteLeft != null)
                _noteLeft.transform.localScale = new Vector3(scale, scale, scale);
            if (_noteRight != null)
                _noteRight.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    
    public bool ColorizeNoteDebris {
        get => BetterBeatSaberConfig.Instance.ColorizeNoteDebris.CurrentValue;
        set => BetterBeatSaberConfig.Instance.ColorizeNoteDebris.SetValue(value);
    }
    
    #region Outlines

    public bool NoteOutlinesEnable {
        get => BetterBeatSaberConfig.Instance.NoteOutlines.Enable.CurrentValue;
        set {
            
            BetterBeatSaberConfig.Instance.NoteOutlines.Enable.SetValue(value);
            
            if (_noteLeft == null)
                return;

            if(_noteLeftOutline != null)
                _noteLeftOutline.enabled = true;
            
            if(_noteRightOutline != null)
                _noteRightOutline.enabled = true;
            
        }
    }
    
    public bool NoteOutlinesGlow {
        get => BetterBeatSaberConfig.Instance.NoteOutlines.Bloom.CurrentValue;
        set {
            BetterBeatSaberConfig.Instance.NoteOutlines.Bloom.SetValue(value);
            if (value && _noteLeftOutline != null)
                _noteLeftOutline.Glowing = value;
            if (value && _noteRightOutline != null)
                _noteRightOutline.Glowing = value;
        }
    }
    
    public float NoteOutlinesWidth {
        get => BetterBeatSaberConfig.Instance.NoteOutlines.Width.CurrentValue;
        set {
            BetterBeatSaberConfig.Instance.NoteOutlines.Width.SetValue(value);
            if (_noteLeftOutline != null)
                _noteLeftOutline.OutlineWidth = value;
            if (_noteRightOutline != null)
                _noteRightOutline.OutlineWidth = value;
        }
    }
    
    public Visibility NoteOutlinesVisibility {
        get => BetterBeatSaberConfig.Instance.NoteOutlines.Visibility;
        set => BetterBeatSaberConfig.Instance.NoteOutlines.Visibility = value;
    }

    public bool ColorizeCustomNoteOutlines {
        get => BetterBeatSaberConfig.Instance.ColorizeCustomNoteOutlines;
        set => BetterBeatSaberConfig.Instance.ColorizeCustomNoteOutlines = value;
    }
    
    #endregion
    
    #endregion

}