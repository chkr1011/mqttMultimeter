using System;

namespace mqttMultimeter.Pages.Subscriptions.State;

public sealed class SubscriptionsPageStateLoader
{
    public static void Apply(SubscriptionsPageViewModel target, SubscriptionsPageState? state)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (state == null)
        {
            // Create one empty demo item.
            target.AddItem();
            return;
        }

        foreach (var subscriptionState in state.Subscriptions)
        {
            target.Items.Collection.Add(CreateItem(target, subscriptionState));
        }
    }

    static SubscriptionItemViewModel CreateItem(SubscriptionsPageViewModel ownerPage, SubscriptionState subscriptionState)
    {
        var item = new SubscriptionItemViewModel(ownerPage)
        {
            Name = subscriptionState.Name ?? string.Empty,
            Topic = subscriptionState.Topic ?? string.Empty,
            NoLocal = subscriptionState.NoLocal,
            RetainAsPublished = subscriptionState.RetainAsPublished,
            QualityOfServiceLevel =
            {
                Value = subscriptionState.QualityOfServiceLevel
            },
            RetainHandling =
            {
                Value = subscriptionState.RetainHandling
            }
        };


        return item;
    }
}