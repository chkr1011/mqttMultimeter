using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace MQTTnet.App.Common;

public abstract class BaseViewModel : ReactiveObject
{
    readonly ViewModelPropertyStore _propertyStore = new();
    
    protected TValue GetValue<TValue>([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        return _propertyStore.GetValue<TValue>(propertyName);
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        this.RaisePropertyChanged(propertyName);
    }
    
    protected void SetValue<TValue>(TValue value, [CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (_propertyStore.SetValue(propertyName, value))
            //this.RaisePropertyChanged(propertyName);
        {
            OnPropertyChanged(propertyName);
        }
    }
}