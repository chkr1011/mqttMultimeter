using mqttMultimeter.Common;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public sealed class DisconnectedReasonViewModel : BaseViewModel
{
    string _additionalInformation = string.Empty;
    string _reason = string.Empty;

    public string AdditionalInformation
    {
        get => _additionalInformation;
        set => this.RaiseAndSetIfChanged(ref _additionalInformation, value);
    }

    public string Reason
    {
        get => _reason;
        set => this.RaiseAndSetIfChanged(ref _reason, value);
    }

    public void Clear()
    {
        Reason = string.Empty;
        AdditionalInformation = string.Empty;
    }
}