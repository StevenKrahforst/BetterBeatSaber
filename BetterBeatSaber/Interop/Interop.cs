using IPA.Loader;
using IPA.Logging;

using Zenject;

namespace BetterBeatSaber.Interop;

public abstract class Interop<T> : IInitializable where T : Interop<T> {

    public static T? Instance { get; private set; }

    protected Logger Logger { get; }
    
    protected abstract string Plugin { get; }

    protected Interop() {
        Instance = (T) this;
        Logger = BetterBeatSaber.Instance.Logger.GetChildLogger($"{GetType().Name} Interop");
    }

    public void Initialize() {
        if (!RunIf())
            return;
        var plugin = PluginManager.GetPluginFromId(Plugin) ?? PluginManager.GetPlugin(Plugin);
        if(plugin != null)
            Init(plugin);
    }

    protected virtual bool RunIf() { return true; }
    
    /// <summary>
    /// Will be called when the plugin is loaded
    /// </summary>
    protected abstract void Init(PluginMetadata pluginMetadata);

}