using System;
using System.Reflection;

using BeatSaberMarkupLanguage;

using BetterBeatSaber.Extensions;

using TMPro;

using Zenject;

namespace BetterBeatSaber.Providers;

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class BetterBloomFontProvider : IInitializable {

    public static BetterBloomFontProvider? Instance { get; private set; }
    
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public TMP_FontAsset? BloomFont => TextMeshProExtensions.BloomFont;
    
    public void Initialize() {
        Instance = this;
    }
    
}