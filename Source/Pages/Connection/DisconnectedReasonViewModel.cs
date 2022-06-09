using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.Connection;

public sealed class DisconnectedReasonViewModel : BaseViewModel
{
    string _reason = string.Empty;
    string _additionalInformation = string.Empty;

    public string Reason
    {
        get => _reason;
        set => this.RaiseAndSetIfChanged(ref _reason, value);
    }

    public string AdditionalInformation
    {
        get => _additionalInformation;
        set => this.RaiseAndSetIfChanged(ref _additionalInformation, value);
    }

    public void Clear()
    {
        Reason = string.Empty;
        AdditionalInformation = string.Empty;
    }
}