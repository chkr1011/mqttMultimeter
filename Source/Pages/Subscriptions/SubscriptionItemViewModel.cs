using System.Threading.Tasks;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using ReactiveUI;

namespace mqttMultimeter.Pages.Subscriptions;

public sealed class SubscriptionItemViewModel : BaseViewModel
{
    string _name = string.Empty;
    bool _noLocal;
    bool _retainAsPublished;
    string _topic = string.Empty;

    public SubscriptionItemViewModel(SubscriptionsPageViewModel ownerPage)
    {
        OwnerPage = ownerPage;
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public bool NoLocal
    {
        get => _noLocal;
        set => this.RaiseAndSetIfChanged(ref _noLocal, value);
    }

    public SubscriptionsPageViewModel OwnerPage { get; }

    public QualityOfServiceLevelSelectorViewModel QualityOfServiceLevel { get; } = new();

    public SubscriptionResponseViewModel Response { get; } = new();

    public bool RetainAsPublished
    {
        get => _retainAsPublished;
        set => this.RaiseAndSetIfChanged(ref _retainAsPublished, value);
    }

    public RetainHandlingSelectorViewModel RetainHandling { get; } = new();

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();

    public Task Subscribe()
    {
        return OwnerPage.SubscribeItem(this);
    }

    public Task Unsubscribe()
    {
        return OwnerPage.UnsubscribeItem(this);
    }
}