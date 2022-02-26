using System.Collections.Generic;

namespace MQTTnetApp.Pages.Publish.State;

public sealed class PublishPageState
{
    public const string Key = "Publish";

    public List<PublishState> Publishes { get; set; } = new();
}