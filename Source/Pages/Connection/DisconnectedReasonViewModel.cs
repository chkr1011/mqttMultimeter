using mqttMultimeter.Common;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public class DisconnectedReasonViewModel : BaseViewModel
{
    public string AdditionalInformation
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = string.Empty;

    public string Reason
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = string.Empty;

    public void Clear()
    {
        Reason = string.Empty;
        AdditionalInformation = string.Empty;
    }
}