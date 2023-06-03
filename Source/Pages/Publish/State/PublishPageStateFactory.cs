using System;
using mqttMultimeter.Services.State.Model;

namespace mqttMultimeter.Pages.Publish.State;

public static class PublishPageStateFactory
{
    public static PublishPageState Create(PublishPageViewModel viewModel)
    {
        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel));
        }

        var state = new PublishPageState();

        foreach (var item in viewModel.Items.Collection)
        {
            var publishState = new PublishState
            {
                Name = item.Name,
                Topic = item.Topic,
                Retain = item.Retain,
                ContentType = item.ContentType,
                ResponseTopic = item.ResponseTopic,
                SubscriptionIdentifier = item.SubscriptionIdentifier,
                TopicAlias = item.TopicAlias,
                MessageExpiryInterval = item.MessageExpiryInterval,
                PayloadFormatIndicator = item.PayloadFormatIndicator.Value,
                QualityOfServiceLevel = item.QualityOfServiceLevel.Value,
                Payload = item.Payload
            };

            foreach (var userProperty in item.UserProperties.Items)
            {
                publishState.UserProperties.Add(new UserProperty
                {
                    Name = userProperty.Name,
                    Value = userProperty.Value
                });
            }

            state.Publishes.Add(publishState);
        }

        return state;
    }
}