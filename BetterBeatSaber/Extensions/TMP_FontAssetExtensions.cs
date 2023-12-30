using TMPro;

#if !PRE_130
using UnityEngine;
#endif

namespace BetterBeatSaber.Extensions;

// ReSharper disable once InconsistentNaming
public static class TMP_FontAssetExtensions {

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