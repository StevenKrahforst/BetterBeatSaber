using System;

namespace BetterBeatSaber.Utilities;

public class ObservableValue<T> {

    public event Action<T>? OnValueChanged;
    
    public T CurrentValue { get; private set; }
    
    public ObservableValue(T defaultValue) {
        CurrentValue = defaultValue;
    }

    public void SetValue(T value) {
        CurrentValue = value;
        OnValueChanged?.Invoke(value);
    }
    
    public static implicit operator T(ObservableValue<T> observableValue) => observableValue.CurrentValue;
    public static implicit operator ObservableValue<T>(T defaultValue) => new(defaultValue);
    
}