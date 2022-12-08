using System;
using ReactiveUI;

namespace mqttMultimeter.Controls;

public sealed class UserPropertyViewModel : ReactiveObject
{
    string _name = string.Empty;
    string _value = string.Empty;

    public UserPropertyViewModel(UserPropertiesViewModel owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public UserPropertiesViewModel Owner { get; }

    public string Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }
}