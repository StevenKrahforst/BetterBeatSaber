using System;

namespace BetterBeatSaber.Utilities;

public sealed class ObservableValue<T>(T defaultValue) {

    public event Action<T>? OnValueChanged;
    
    public T CurrentValue { get; private set; } = defaultValue;

    public void SetValue(T value, bool invokeChange = true, bool invokeChangeOnlyIfDifferent = true) {
        
        if (invokeChangeOnlyIfDifferent && Equals(CurrentValue, value))
            return;
        
        CurrentValue = value;
        
        if (invokeChange)
            OnValueChanged?.Invoke(CurrentValue);
        
    }
    
    public static implicit operator T(ObservableValue<T> observableValue) => observableValue.CurrentValue;
    public static implicit operator ObservableValue<T>(T defaultValue) => new(defaultValue);
    
}