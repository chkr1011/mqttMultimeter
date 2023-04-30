using mqttMultimeter.Common;
using ReactiveUI;

namespace mqttMultimeter.Controls;

public sealed class ProgressIndicatorViewModel : BaseViewModel
{
    object? _message;

    public object? Message
    {
        get => _message;
        set => this.RaiseAndSetIfChanged(ref _message, value);
    }

    public static ProgressIndicatorViewModel Create(string message)
    {
        return new ProgressIndicatorViewModel
        {
            Message = message
        };
    }
}