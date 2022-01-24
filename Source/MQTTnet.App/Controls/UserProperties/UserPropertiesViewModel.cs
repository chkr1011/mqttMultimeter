using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MQTTnet.App.Common;
using MQTTnet.Packets;

namespace MQTTnet.App.Controls.UserProperties;

public sealed class UserPropertiesViewModel : BaseViewModel
{
    public bool IsReadOnly
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public ObservableCollection<UserPropertyViewModel> Items { get; } = new();

    public void AddItem()
    {
        Items.Add(new UserPropertyViewModel(this));
    }

    public void ClearItems()
    {
        Items.Clear();
    }

    public void Load(List<MqttUserProperty> userProperties)
    {
        Items.Clear();

        foreach (var userProperty in userProperties)
        {
            Items.Add(new UserPropertyViewModel(this)
            {
                Name = userProperty.Name, Value = userProperty.Value
            });
        }
    }

    public void RemoveItem(UserPropertyViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        Items.Remove(item);
    }
}