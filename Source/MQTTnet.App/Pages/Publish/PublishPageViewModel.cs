using System;
using System.Collections.ObjectModel;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;

namespace MQTTnet.App.Pages.Publish
{
    public sealed class PublishPageViewModel : BasePageViewModel
    {
        readonly MqttClientService _mqttClientService;

        public PublishPageViewModel(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

            Header = new TextViewModel("Publish");

            Publishes.Add(new PublishOptionsViewModel(_mqttClientService)
            {
                Topic = "Test"
            });
        }

        public ObservableCollection<PublishOptionsViewModel> Publishes { get; } = new ObservableCollection<PublishOptionsViewModel>();
    }
}
