using System.Collections.Generic;
using MQTTnet;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CollectionNeverQueried.Global

namespace MQTTnetApp.Pages.Inflight.Export.Model;

public sealed class InflightPageExport
{
    public List<MqttApplicationMessage>? Messages { get; set; }
}