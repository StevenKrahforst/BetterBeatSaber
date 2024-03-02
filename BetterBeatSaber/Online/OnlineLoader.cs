using System.IO;
using System.IO.Compression;
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

        #if DEBUG // Update to Local !!!
        using var stream = await new HttpClient().GetStreamAsync("https://github.com/BetterBeatSaber/BetterBeatSaber/releases/latest/download/Better.Beat.Saber.Online.zip");
        #else
        using var stream = await new HttpClient().GetStreamAsync("https://github.com/BetterBeatSaber/BetterBeatSaber/releases/latest/download/Better.Beat.Saber.Online.zip");
        #endif
        
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        
        var entry = archive.GetEntry("Better Beat Saber Online.dll");
        if (entry == null)
            return;
        
        using var rawStream = entry.Open();
        using var memoryStream = new MemoryStream();
        
        await rawStream.CopyToAsync(memoryStream);
        
        Assembly = Assembly.Load(memoryStream.ToArray());

        Instance = Assembly
            .GetType("BetterBeatSaber.Online.BetterBeatSaberOnline")?
            .GetConstructor([ typeof(Logger), typeof(Zenjector), typeof(MixinManager) ])?
            .Invoke([ BetterBeatSaber.Instance.Logger, BetterBeatSaber.Instance.Zenjector, BetterBeatSaber.Instance.MixinManager ]);

    }
    
    internal static void Start() =>
        Instance?.GetType().GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(Instance, []);

    internal static void Exit() =>
        Instance?.GetType().GetMethod("Exit", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(Instance, []);
    
}