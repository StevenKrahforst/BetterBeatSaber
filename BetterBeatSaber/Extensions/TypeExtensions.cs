using System;
using System.IO;

namespace BetterBeatSaber.Extensions; 

public static class TypeExtensions {

    public static string GetDefinitionResourceName(this Type type) =>
        type.Namespace + "." + type.Name + ".xml";

    public static string ReadDefinition(this Type type, string? fallback = null) {
        using var stream = type.Assembly.GetManifestResourceStream(type.GetDefinitionResourceName());
        if (stream == null)
            return fallback ?? string.Empty;
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }

}