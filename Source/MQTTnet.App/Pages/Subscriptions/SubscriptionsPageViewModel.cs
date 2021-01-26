using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Main;
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

        public async Task CreateSubscription()
        {
            try
            {
                var viewModel = new SubscriptionEditorViewModel();

                var window = new SubscriptionEditorView
                {
                    Title = "Create subscription",
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ShowInTaskbar = false,
                    CanResize = false,
                    DataContext = viewModel
                };

                await window.ShowDialog(MainWindowView.Instance);

                if (viewModel.Accepted)
                {
                    await _mqttClientService.Subscribe(viewModel).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {

            }

        }

        public void Clear()
        {
            ReceivedApplicationMessages.Clear();
        }
    }
}
