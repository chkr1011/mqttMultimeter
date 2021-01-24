using System.Collections.ObjectModel;
using MQTTnet.App.Common;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Publish;
using MQTTnet.App.Pages.Subscriptions;

namespace MQTTnet.App.Main
{
    public sealed class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(
            ConnectionPageViewModel connectionPageViewModel,
            SubscriptionsPageViewModel subscriptionsPageViewModel,
            PublishPageViewModel publishPageViewModel)
        {
            Pages.Add(connectionPageViewModel);
            Pages.Add(subscriptionsPageViewModel);
            Pages.Add(publishPageViewModel);
        }

        public ObservableCollection<BaseViewModel> Pages { get; } = new ObservableCollection<BaseViewModel>();
    }
}
