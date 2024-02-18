using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using BetterBeatSaber.Config.Converters;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BetterBeatSaber.Config;

public class Config;

public abstract class Config<T> : Config where T : Config<T> {

    public static T Instance { get; private set; } = null!;
    
    // ReSharper disable once StaticMemberInGenericType
    public static readonly JsonSerializerSettings SerializerSettings = new() {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        DefaultValueHandling = DefaultValueHandling.Populate,
        TypeNameHandling = TypeNameHandling.Auto,
        ObjectCreationHandling = ObjectCreationHandling.Reuse,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Converters = new List<JsonConverter> {
            new StringEnumConverter(),
            new ColorConverter(),
            new Vector3Converter(),
            new QuaternionConverter(),
            new ObservableValueConverter<bool>(),
            new ObservableValueConverter<float>()
        },
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };
    
    [JsonIgnore]
    public string Path { get; private set; }

    [JsonIgnore]
    public FileSystemWatcher Watcher { get; } = new() {
        NotifyFilter = NotifyFilters.LastWrite,
        EnableRaisingEvents = true
    };
    
    [JsonIgnore]
    private DateTime? LastSaveTime { get; set; }
    
    [JsonIgnore]
    private DateTime? LastInvokeTime { get; set; }
    
    protected Config(string name) {
        
        Instance = (T) this;
        
        Path = System.IO.Path.Combine(Environment.CurrentDirectory, "UserData", $"{name}.json");
        
        Watcher.Changed += OnChanged;
        
        Watcher.Path = System.IO.Path.Combine(Environment.CurrentDirectory, "UserData");
        Watcher.Filter = $"{name}.json";
        
        Load();
        Save();
        
    }

    private void OnChanged(object _, FileSystemEventArgs args) {
        
        if (args.ChangeType != WatcherChangeTypes.Changed)
            return;
        
        if(DateTime.Now - LastSaveTime < TimeSpan.FromSeconds(3) ||
           DateTime.Now - LastInvokeTime < TimeSpan.FromMilliseconds(25))
            return;

        SharedCoroutineStarter.instance.StartCoroutine(LoadAsync());
        
        Load();
        
        LastInvokeTime = DateTime.Now;
        
    }

    public void Load() {
        if (File.Exists(Path)) {
            try {
                JsonConvert.PopulateObject(File.ReadAllText(Path), this, SerializerSettings);
            } catch (Exception exception) {
                BetterBeatSaber.Instance.Logger.Error("Failed to load config file! Using default values instead.");
                BetterBeatSaber.Instance.Logger.Error(exception);
            }
        } else Save();
    }

    public IEnumerator LoadAsync() {
        Load();
        yield break;
    }

    public void Save() {
        File.WriteAllText(Path, JsonConvert.SerializeObject(this, SerializerSettings));
        LastSaveTime = DateTime.Now;
    }
    
    // Resources.FindObjectsOfTypeAll<MenuTransitionsHelper>().FirstOrDefault()?.RestartGame();

}