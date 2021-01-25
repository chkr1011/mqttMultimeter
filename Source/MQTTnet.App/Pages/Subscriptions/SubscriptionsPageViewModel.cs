using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;
using MQTTnet.Client.Receiving;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscriptionsPageViewModel : BasePageViewModel, IMqttApplicationMessageReceivedHandler
    {
        readonly MqttClientService _mqttClientService;

        int _messageId;


        public SubscriptionsPageViewModel(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

            mqttClientService.RegisterApplicationMessageReceivedHandler(this);

            Header = "Subscriptions";
        }

        public ViewModelCollection<ReceivedApplicationMessageViewModel> ReceivedApplicationMessages { get; } = new ViewModelCollection<ReceivedApplicationMessageViewModel>();

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            return Dispatcher.UIThread.InvokeAsync(() =>
            {
                ReceivedApplicationMessages.Add(new ReceivedApplicationMessageViewModel(_messageId++, eventArgs.ApplicationMessage));
            });
        }

        public void Clear()
        {
            ReceivedApplicationMessages.Clear();
        }
    }
}
