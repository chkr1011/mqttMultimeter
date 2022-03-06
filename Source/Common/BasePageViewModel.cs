using ReactiveUI;

namespace MQTTnetApp.Common;

public abstract class BasePageViewModel : BaseViewModel
{
    object? _overlayContent;

    public object? OverlayContent
    {
        get => _overlayContent;
        set => this.RaiseAndSetIfChanged(ref _overlayContent, value);
    }
}