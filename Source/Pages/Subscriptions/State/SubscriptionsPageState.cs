using System.Collections.Generic;

namespace mqttMultimeter.Pages.Subscriptions.State;

public sealed class SubscriptionsPageState
{
    public const string Key = "Subscriptions";

    public List<SubscriptionState> Subscriptions { get; set; } = new();
}