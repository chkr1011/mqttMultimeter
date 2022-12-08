using System.Text.Json.Serialization;

namespace mqttMultimeter.Services.Updates.Model;

public sealed class Release
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}