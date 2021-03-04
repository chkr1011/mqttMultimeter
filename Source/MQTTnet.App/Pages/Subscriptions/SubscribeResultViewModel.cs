using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscribeResultViewModel : BaseViewModel
    {
        public string Topic { get; set; } = string.Empty;

        public string Response { get; set; } = string.Empty;

        public string ResponseCode { get; set; } = string.Empty;

        public bool Succeeded { get; set; }
    }
}