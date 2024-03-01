using System;

namespace BetterBeatSaber.Utilities;

public class ObservableValue<T>(T defaultValue) {

    public event Action<T>? OnValueChanged;
    
    public virtual T CurrentValue { get; private set; } = defaultValue;
    
    public virtual void SetValue(T value, bool invokeChange = true, bool invokeChangeOnlyIfDifferent = true) {
        
        if (invokeChangeOnlyIfDifferent && Equals(CurrentValue, value))
            return;
        
        CurrentValue = value;
        
        if (invokeChange)
            OnValueChanged?.Invoke(CurrentValue);
        
    }
    
    public static implicit operator T(ObservableValue<T> observableValue) => observableValue.CurrentValue;
    public static implicit operator ObservableValue<T>(T defaultValue) => new(defaultValue);
    
}