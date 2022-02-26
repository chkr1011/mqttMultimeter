using System;

namespace MQTTnetApp.Pages.Publish.State;

public static class PublishPageStateLoader
{
    public static void Apply(PublishPageViewModel target, PublishPageState? state)
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

        foreach (var publishState in state.Publishes)
        {
            target.Items.Collection.Add(CreateItem(target, publishState));
        }
    }

    static PublishItemViewModel CreateItem(PublishPageViewModel ownerPage, PublishState publishState)
    {
        var item = new PublishItemViewModel(ownerPage)
        {
            Name = publishState.Name ?? string.Empty,
            Topic = publishState.Topic,
            Retain = publishState.Retain,
            ContentType = publishState.ContentType,
            QualityOfServiceLevel =
            {
                Value = publishState.QualityOfServiceLevel
            }
        };

        return item;
    }
}