using System;
using MQTTnet.App.Common;

namespace MQTTnet.App.Controls.UserProperties;

public sealed class UserPropertyViewModel : BaseViewModel
{
    public UserPropertyViewModel(UserPropertiesViewModel owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public string Name
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public UserPropertiesViewModel Owner { get; }

    public string Value
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
}