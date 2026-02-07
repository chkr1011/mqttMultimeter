using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using mqttMultimeter.Common;
using MQTTnet.Packets;
using ReactiveUI;

namespace mqttMultimeter.Controls;

public class UserPropertiesViewModel : BaseViewModel
{
    public bool IsReadOnly
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<UserPropertyViewModel> Items { get; } = [];

    public void AddEmptyItem()
    {
        AddItem(string.Empty, string.Empty);
    }

    public void AddItem(string name, string value)
    {
        var item = new UserPropertyViewModel(this)
        {
            Name = name,
            Value = value
        };

        Items.Add(item);
    }

    public void ClearItems()
    {
        Items.Clear();
    }

    public void Load(IReadOnlyCollection<MqttUserProperty>? userProperties)
    {
        Items.Clear();

        if (userProperties == null)
        {
            return;
        }

        foreach (var userProperty in userProperties)
        {
            Items.Add(new UserPropertyViewModel(this)
            {
                Name = userProperty.Name,
                Value = userProperty.ReadValueAsString()
            });
        }
    }

    public void RemoveItem(UserPropertyViewModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Items.Remove(item);
    }
}