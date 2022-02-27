using ReactiveUI;

namespace MQTTnetApp.Common;

public abstract class BasePageViewModel : BaseViewModel
{
    object? _overlayContent;

    public bool IsOverlayVisible => _overlayContent != null;

    public object? OverlayContent
    {
        get => _overlayContent;
        set
        {
            this.RaiseAndSetIfChanged(ref _overlayContent, value);
            this.RaisePropertyChanged(nameof(IsOverlayVisible));
        }
    }
}

public abstract class BaseViewModel : ReactiveObject
{
}