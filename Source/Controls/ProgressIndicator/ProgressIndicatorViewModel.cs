﻿using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Controls;

public sealed class ProgressIndicatorViewModel : BaseViewModel
{
    object? _message;

    public object? Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }
}