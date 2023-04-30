using System;
using ReactiveUI;

namespace mqttMultimeter.Common;

public abstract class BasePageViewModel : BaseViewModel
{
    object? _overlayContent;

    public event EventHandler? ActivationRequested;

    public object? OverlayContent
    {
        get => _overlayContent;
        set => this.RaiseAndSetIfChanged(ref _overlayContent, value);
    }

    public void RequestActivation()
    {
        ActivationRequested?.Invoke(this, EventArgs.Empty);
    }
}