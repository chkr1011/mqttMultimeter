using System;
using ReactiveUI;

namespace mqttMultimeter.Controls;

public class UserPropertyViewModel(UserPropertiesViewModel owner) : ReactiveObject
{
    public string Name
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = string.Empty;

    public UserPropertiesViewModel Owner { get; } = owner ?? throw new ArgumentNullException(nameof(owner));

    public string Value
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = string.Empty;
}