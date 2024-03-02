using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BetterBeatSaber.Shared.Extensions;

public static class HttpExtensions {

    public static readonly JsonSerializerSettings DefaultSerializerSettings = new() {
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        },
        NullValueHandling = NullValueHandling.Ignore
    };
    
    public static async Task<T?> ReadAsJsonAsync<T>(this HttpContent httpContent, JsonSerializerSettings? serializerSettings = null) {
        var response = await httpContent.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(response, serializerSettings ?? DefaultSerializerSettings);
    }
    
    public static async Task<T?> GetJsonAsync<T>(this HttpClient httpClient, string url, bool ensureSuccessStatusCode = false, JsonSerializerSettings? serializerSettings = null) {
        var response = await httpClient.GetAsync(url);
        if(ensureSuccessStatusCode)
            response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), serializerSettings ?? DefaultSerializerSettings);
    }

}