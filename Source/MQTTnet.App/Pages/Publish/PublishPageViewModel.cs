using System;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;

namespace MQTTnet.App.Pages.Publish
{
    public sealed class PublishPageViewModel : BasePageViewModel
    {
        readonly MqttClientService _mqttClientService;

        public PublishPageViewModel([NotNull] MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

            Header = new TextViewModel("Publish");

            Publishes.Add(new PublishViewModel(_mqttClientService)
            {
                Topic = "Test"
            });
        }

        public ObservableCollection<PublishViewModel> Publishes { get; } = new ObservableCollection<PublishViewModel>();
    }
}
