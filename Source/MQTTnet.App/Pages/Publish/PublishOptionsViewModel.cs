using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.App.Common;
using MQTTnet.App.Common.QualityOfServiceLevel;
using MQTTnet.App.Services.Client;

namespace MQTTnet.App.Pages.Publish
{
    public sealed class PublishOptionsViewModel : BaseViewModel
    {
        readonly MqttClientService _mqttClientService;

        public PublishOptionsViewModel(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

            Payload = string.Empty;
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Topic
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public QualityOfServiceLevelSelectorViewModel QualityOfServiceLevel { get; } = new QualityOfServiceLevelSelectorViewModel();

        public bool Retain
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string Payload
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public async Task Publish()
        {
            try
            {
                var result = await _mqttClientService.Publish(this);
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
        }

        public async Task Delete()
        {

        }

        public byte[] GeneratePayload()
        {
            return Encoding.UTF8.GetBytes(Payload);
        }
    }
}