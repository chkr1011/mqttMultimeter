using System.Collections.Generic;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CollectionNeverQueried.Global

namespace mqttMultimeter.Pages.Inflight.Export.Model;

public sealed class InflightPageExport
{
    public List<InflightPageExportMessage>? Messages { get; set; }
}