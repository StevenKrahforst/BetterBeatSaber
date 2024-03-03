using System.Collections.Generic;

using BetterBeatSaber.Shared.Extensions;

using Newtonsoft.Json;

using UnityEngine.Networking;

namespace BetterBeatSaber.Online.Api;

public sealed class ApiRequest : UnityWebRequest {

    #if !DEBUG
    private const string BaseUrl = "http://localhost:5182";
    #else
    private const string BaseUrl = "https://api.betterbs.xyz";
    #endif
    
    public bool Failed => isHttpError || isNetworkError;

    public bool IsDone => isDone;
    
    public string ContentString => downloadHandler.text;
    public byte[] ContentBytes => downloadHandler.data;
    
    private string _path = string.Empty;
    public string Path {
        get => _path;
        set {
            _path = value;
            UpdateUrl();
        }
    }
    
    private Dictionary<string, string>? _queryParameters = new();
    public Dictionary<string, string>? QueryParameters {
        get => _queryParameters;
        set {
            _queryParameters = value;
            UpdateUrl();
        }
    }
    
    public ApiRequest(string path, string method = kHttpVerbGET, Dictionary<string, string>? queryParameters = null) {
        this.method = method;
        Path = path;
        QueryParameters = queryParameters;
        downloadHandler = new DownloadHandlerBuffer();
    }

    public T? ContentAs<T>(JsonSerializerSettings? settings = null) =>
        JsonConvert.DeserializeObject<T>(ContentString, settings);

    private void UpdateUrl() =>
        url = BuildUrl(_path, _queryParameters);

    private static string BuildUrl(string path, Dictionary<string, string>? query = null) =>
        $"{BaseUrl}{path}{query.BuildQueryString()}";

}