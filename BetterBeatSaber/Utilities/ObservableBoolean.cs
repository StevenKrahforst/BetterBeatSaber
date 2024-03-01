using IPA.Loader;

namespace BetterBeatSaber.Utilities;

public class ObservableBoolean(bool defaultValue, string? conflictsWith = null) : ObservableValue<bool>(defaultValue) {

    public string? ConflictsWith { get; } = conflictsWith;

    public bool Conflict => ConflictsWith != null && PluginManager.GetPluginFromId(ConflictsWith) != null;

    public override bool CurrentValue => base.CurrentValue && !Conflict;

    public override void SetValue(bool value, bool invokeChange = true, bool invokeChangeOnlyIfDifferent = true) {
        if(!Conflict)
            base.SetValue(value, invokeChange, invokeChangeOnlyIfDifferent);
    }

}