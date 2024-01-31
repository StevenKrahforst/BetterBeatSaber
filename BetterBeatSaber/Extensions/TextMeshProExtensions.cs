using TMPro;

using UnityEngine;

namespace BetterBeatSaber.Extensions;

public static class TextMeshProExtensions {

    public static void ApplyGradient(this TMP_Text text, Color start, Color end) {
        
        text.ForceMeshUpdate();
        
        var length = text.textInfo.characterInfo.Length;
        
        var steps = start.Steps(end, length);
        var gradients = new VertexGradient[length];
        for (var index = 0; index < length; index++) {
            
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
    
    public static TMP_FontAsset Clone(this TMP_FontAsset original) {

#if PRE_130
        return TMP_FontAsset.CreateFontAsset(original.sourceFontFile);
#else
        
        if (original.atlasPopulationMode == AtlasPopulationMode.Dynamic && original.sourceFontFile != null)
            return TMP_FontAsset.CreateFontAsset(original.sourceFontFile);
      
        var instance = ScriptableObject.CreateInstance<TMP_FontAsset>();
        
        instance.SetField("m_Version", original.version);
        instance.SetField("m_FaceInfo", original.faceInfo);
        
        instance.atlasPopulationMode = original.atlasPopulationMode;
        instance.atlas = original.atlas;
        instance.atlasTextures = original.atlasTextures;
        
        instance.SetField("m_AtlasWidth", original.atlasWidth);
        instance.SetField("m_AtlasHeight", original.atlasHeight);
        instance.SetField("m_AtlasPadding", original.atlasPadding);
        instance.SetField("m_AtlasRenderMode", original.atlasRenderMode);

        #if !PRE_130
        instance.isMultiAtlasTexturesEnabled = original.isMultiAtlasTexturesEnabled;
        #endif
        
        instance.material = new Material(original.material);
        
        instance.ReadFontAssetDefinition();
        
        return instance;

#endif
        
    }

}