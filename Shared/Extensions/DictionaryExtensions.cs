using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterBeatSaber.Shared.Extensions;

public static class DictionaryExtensions {

    public static string BuildQueryString(this Dictionary<string, string>? dictionary) {
        
        if (dictionary == null || dictionary.Count == 0)
            return string.Empty;
        
        return "?" + string.Join("&", dictionary.Select(item => $"{HttpUtility.UrlEncode(item.Key)}={HttpUtility.UrlEncode(item.Value)}"));
        
    }

}