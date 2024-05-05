using BeatSaberMarkupLanguage;

using TMPro;

using UnityEngine;

namespace BetterBeatSaber.Extensions;

public static class TextMeshProExtensions {

    internal static TMP_FontAsset? BloomFont { get; set; } = null!;
    
    public static void ApplyGradient(this TMP_Text text, Color start, Color end, int until = -1) {
        
        text.ForceMeshUpdate();
        
        until = until == -1 || until > text.textInfo.characterCount ? text.textInfo.characterCount : until;
        
        var steps = start.Steps(end, until);
        var gradients = new VertexGradient[until];
        for (var index = 0; index < until; index++) {
            
            gradients[index] = new VertexGradient(steps[index], steps[index + 1], steps[index], steps[index + 1]);
            
            var characterInfo = text.textInfo.characterInfo[index];
            if (!characterInfo.isVisible || characterInfo.character == ' ')
                continue;
            
            var colors = text.textInfo.meshInfo[characterInfo.materialReferenceIndex].colors32;
            
            var vertexIndex = text.textInfo.characterInfo[index].vertexIndex;
            
            colors[vertexIndex + 0] = gradients[index].bottomLeft;
            colors[vertexIndex + 1] = gradients[index].topLeft;
            colors[vertexIndex + 2] = gradients[index].bottomRight;
            colors[vertexIndex + 3] = gradients[index].topRight;
            
        }
            
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        
    }

    public static void SetBloom(this TextMeshPro text, bool enable = true) {
        text.font = enable ? BloomFont ?? BeatSaberUI.MainTextFont : BeatSaberUI.MainTextFont;
    }
    
}