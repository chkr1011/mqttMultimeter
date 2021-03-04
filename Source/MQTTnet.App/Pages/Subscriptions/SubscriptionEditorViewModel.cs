using System;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;
using MQTTnet.Client.Subscribing;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscriptionEditorViewModel : BaseWizardViewModel
    {
        readonly MqttClientService _mqttClientService;

        public SubscriptionEditorViewModel(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

            ActivePage = ConfigurationPage;
        }

        public event EventHandler Completed;

        public SubscriptionOptionsPageViewModel ConfigurationPage { get; } = new SubscriptionOptionsPageViewModel();

        public bool Subscribed { get; private set; }

        public async Task Accept()
        {
            try
            {
                if (ActivePage == ConfigurationPage)
                {
                    if (!ConfigurationPage.Validate())
                    {
                        return;
                    }

                    // TODO: Show some animation.

                    var result = await _mqttClientService.Subscribe(ConfigurationPage);

                    // Since this app is only able to deal with a single subscription at a time
                    // we can show the result in a 1:1 base.
                    var resultItem = result.Items.First();

                    var resultViewModel = new SubscribeResultViewModel
                    {
                        Topic = resultItem.TopicFilter.Topic,
                        Response = resultItem.ResultCode.ToString(),
                        ResponseCode = ((int)resultItem.ResultCode).ToString(),
                        Succeeded = resultItem.ResultCode <= MqttClientSubscribeResultCode.GrantedQoS2
                    };

                    ActivePage = resultViewModel;

                    Subscribed = resultViewModel.Succeeded;
                }
                else if (ActivePage is SubscribeResultViewModel)
                {
                    Cancel();
                }
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
        }

        public void Cancel()
        {
            Completed.Invoke(this, EventArgs.Empty);
        }
    }
}