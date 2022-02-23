using System.Collections.Generic;

namespace MQTTnetApp.Pages.Subscriptions.State;

public sealed class SubscriptionsPageState
{
    public List<SubscriptionState> Subscriptions { get; set; } = new();
}