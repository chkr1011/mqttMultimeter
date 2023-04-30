using System;

namespace mqttMultimeter.Pages.Subscriptions.State;

public static class SubscriptionsPageStateFactory
{
    public static SubscriptionsPageState Create(SubscriptionsPageViewModel viewModel)
    {
        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel));
        }

        var state = new SubscriptionsPageState();

        foreach (var item in viewModel.Items.Collection)
        {
            state.Subscriptions.Add(new SubscriptionState
            {
                Name = item.Name,
                Topic = item.Topic,
                NoLocal = item.NoLocal,
                RetainAsPublished = item.RetainAsPublished,
                QualityOfServiceLevel = item.QualityOfServiceLevel.Value,
                RetainHandling = item.RetainHandling.Value
            });
        }

        return state;
    }
}