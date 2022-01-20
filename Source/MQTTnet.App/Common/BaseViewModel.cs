using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MQTTnet.App.Common;

public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    readonly Dictionary<string, IEnumerable> _errors = new();
    readonly ViewModelPropertyStore _propertyStore = new();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool HasErrors => _errors.Count > 0;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrEmpty(propertyName)) return default!;

        if (_errors.TryGetValue(propertyName, out var errors)) return errors;

        return default!;
    }

    protected void ClearErrors()
    {
        _errors.Clear();

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(string.Empty));

        OnPropertyChanged(nameof(HasErrors));
    }

    protected TValue GetValue<TValue>([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        return _propertyStore.GetValue<TValue>(propertyName);
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetErrors(string propertyName, params string[] errors)
    {
        SetErrors(propertyName, (ICollection) errors);
    }

    protected void SetErrors(string propertyName, ICollection? errors)
    {
        if (errors == null || errors.Count == 0)
            _errors.Remove(propertyName);
        else
            _errors[propertyName] = errors;

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        OnPropertyChanged(nameof(HasErrors));
    }

    protected void SetValue<TValue>(TValue value, [CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        if (_propertyStore.SetValue(propertyName, value))
            //this.RaisePropertyChanged(propertyName);
            OnPropertyChanged(propertyName);
    }
}