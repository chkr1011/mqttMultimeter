using System;
using MQTTnet.App.Common;
using ReactiveUI;

namespace MQTTnet.App.Controls;

public sealed class UserPropertyViewModel : BaseViewModel
{
    string _name;
    string _value;

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