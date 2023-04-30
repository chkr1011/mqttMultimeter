using System;
using ReactiveUI;

namespace mqttMultimeter.Common;

public class EnumViewModel<TEnum> : BaseViewModel where TEnum : Enum
{
    object? _displayValue;

    public EnumViewModel(object? displayValue, TEnum value)
    {
        Value = value;
        DisplayValue = displayValue;
    }

    public EnumViewModel(TEnum value)
    {
        Value = value;
        DisplayValue = Convert.ToString(value) ?? string.Empty;
    }

    public object? DisplayValue
    {
        get => _displayValue;
        set => this.RaiseAndSetIfChanged(ref _displayValue, value);
    }

    public TEnum Value { get; }
}