using System.Text.Json;

namespace MQTTnetApp.Services.Data;

public sealed class JsonSerializerService
{
    readonly JsonSerializerOptions _serializationOptions = new()
    {
        WriteIndented = true
    };

    public TGraph? Deserialize<TGraph>(string json)
    {
        return JsonSerializer.Deserialize<TGraph>(json);
    }

    public string Serialize(object? graph)
    {
        return JsonSerializer.Serialize(graph, _serializationOptions);
    }
}