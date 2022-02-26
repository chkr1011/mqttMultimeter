using System;

namespace MQTTnetApp.Pages.Publish.State;

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
            state.Publishes.Add(new PublishState
            {
                Name = item.Name,
                Topic = item.Topic,
                Retain = item.Retain,
                ContentType = item.ContentType,
                PayloadFormatIndicator = item.PayloadFormatIndicator.Value,
                QualityOfServiceLevel = item.QualityOfServiceLevel.Value
            });
        }

        return state;
    }
}