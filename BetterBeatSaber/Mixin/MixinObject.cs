using BetterBeatSaber.Utilities;

namespace BetterBeatSaber.Mixin;

internal abstract class MixinObject(MixinManager mixinManager) {

    protected MixinManager MixinManager { get; } = mixinManager;
    
    internal void ListenToChanges(ObservableValue<bool> toggleObservable) =>
        toggleObservable.OnValueChanged += OnToggleChanged;
    
    internal void UnlistenToChanges(ObservableValue<bool> toggleObservable) =>
        toggleObservable.OnValueChanged -= OnToggleChanged;
    
    protected virtual void OnToggleChanged(bool value) {
        if(value)
            Patch();
        else
            Unpatch();
    }

    internal abstract void Patch();
    internal abstract void Unpatch();

}