using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using mqttMultimeter.Common;
using MQTTnet.Packets;
using ReactiveUI;

namespace mqttMultimeter.Controls;

public sealed class UserPropertiesViewModel : BaseViewModel
{
    bool _isReadOnly;

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set => this.RaiseAndSetIfChanged(ref _isReadOnly, value);
    }

    public ObservableCollection<UserPropertyViewModel> Items { get; } = new();

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
                Value = userProperty.Value
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