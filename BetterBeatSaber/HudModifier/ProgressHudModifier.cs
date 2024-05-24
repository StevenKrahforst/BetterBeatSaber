using BetterBeatSaber.Providers;

using HMUI;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

using Object = UnityEngine.Object;

namespace BetterBeatSaber.HudModifier; 

internal sealed class ProgressHudModifier : IHudModifier, ITickable {

    [Inject, UsedImplicitly]
    private readonly SongProgressUIController _songProgressUIController = null!;

    [Inject, UsedImplicitly]
    private readonly AudioTimeSyncController _audioTimeSyncController = null!;

    [Inject, UsedImplicitly]
    private readonly MaterialProvider _materialProvider = null!;

    private ImageView? _progressBar;

    public void Initialize() {
        
        var progress = _songProgressUIController.transform.Find("Progress");
        if (progress == null)
            return;

        Object.DestroyImmediate(_songProgressUIController.transform.Find("Slider").gameObject);

        _progressBar = progress.gameObject.GetComponent<ImageView>();

        if (BetterBeatSaberConfig.Instance.ProgressHudModifier.Glow)
            _progressBar.material = _materialProvider.DistanceFieldMaterial;
        
    }

    public void Tick() {
        if (_progressBar != null)
            _progressBar.color = Color.Lerp(Color.red, Color.green, _audioTimeSyncController.songTime / _audioTimeSyncController.songLength);
    }

}