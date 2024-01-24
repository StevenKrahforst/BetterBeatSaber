using System.Linq;

using BetterBeatSaber.Extensions;

using TMPro;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Providers;

public sealed class BetterBloomFontProvider : IInitializable {

    public static BetterBloomFontProvider? Instance { get; private set; }
    
    [Inject]
    private readonly MaterialProvider _materialProvider = null!;
    
    public TMP_FontAsset? DefaultFont { get; private set; }
    public TMP_FontAsset? BloomFont { get; private set; }

    public void Initialize() {

        Instance = this;
        
        // ReSharper disable once StringLiteralTypo
        DefaultFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(fontAsset => fontAsset.name == "Teko-Medium SDF");

        if (DefaultFont == null)
            return;
        
        BloomFont = DefaultFont.Clone();
        BloomFont.name = "Font Bloom";
        BloomFont.material = _materialProvider.DistanceFieldMaterial;

    }
    
    public void SetBloom(ref TMP_Text text, bool enable) {
        text.font = enable ? BloomFont : DefaultFont;
    }
    public void SetBloom(ref TextMeshPro text, bool enable) {
        text.font = enable ? BloomFont : DefaultFont;
    }

}