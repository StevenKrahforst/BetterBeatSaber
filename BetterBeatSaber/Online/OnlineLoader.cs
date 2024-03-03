using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using BetterBeatSaber.Extensions;
using BetterBeatSaber.Mixin;

using IPA.Logging;

using SiraUtil.Zenject;

namespace BetterBeatSaber.Online;

internal static class OnlineLoader {

    internal static Assembly? Assembly { get; private set; }
    internal static object? Instance { get; private set; }

    internal static void Load() =>
        Utilities.SharedCoroutineStarter.Instance.StartCoroutine(LoadAsync().AsCoroutine());

    private static async Task LoadAsync() {

        #if !DEBUG
        
        var raw = await new HttpClient().GetByteArrayAsync("http://localhost:5182/download");
        if (raw is null or { Length: 0 })
            return;
        
        Assembly = Assembly.Load(raw);
        
        #else
        
        using var stream = await new HttpClient().GetStreamAsync("https://github.com/BetterBeatSaber/BetterBeatSaber/releases/latest/download/Better.Beat.Saber.Online.zip");

        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        
        var entry = archive.GetEntry("Better Beat Saber Online.dll");
        if (entry == null)
            return;
        
        using var rawStream = entry.Open();
        using var memoryStream = new MemoryStream();
        
        await rawStream.CopyToAsync(memoryStream);
        
        Assembly = Assembly.Load(memoryStream.ToArray());

        #endif

        Instance = Assembly
            .GetType("BetterBeatSaber.Online.BetterBeatSaberOnline")?
            .GetConstructor([ typeof(Logger), typeof(Zenjector), typeof(MixinManager) ])?
            .Invoke([ BetterBeatSaber.Instance.Logger, BetterBeatSaber.Instance.Zenjector, BetterBeatSaber.Instance.MixinManager ]);

        InvokeMethod("Init");
        
    }
    
    internal static void Start() =>
        InvokeMethod(nameof(Start));

    internal static void Exit() =>
        InvokeMethod(nameof(Exit));
    
    private static void InvokeMethod(string name) =>
        Instance?.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(Instance, []);
    
}