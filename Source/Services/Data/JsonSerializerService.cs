using System.Text.Json;
using System.Text.Json.Nodes;

namespace mqttMultimeter.Services.Data;

public sealed class JsonSerializerService
{
    readonly JsonSerializerOptions _serializationOptions = new()
    {
        WriteIndented = true
    };

    public JsonSerializerService()
    {
        Instance = this;
    }

    // This instance is used within control where DI is not possible.
    public static JsonSerializerService? Instance { get; private set; }

    public TGraph? Deserialize<TGraph>(string json)
    {
        return JsonSerializer.Deserialize<TGraph>(json);
    }

    public string Format(string json)
    {
        var jsonNode = JsonNode.Parse(json);
        return JsonSerializer.Serialize(jsonNode, _serializationOptions);
    }

    public string Serialize(object? graph)
    {
        return JsonSerializer.Serialize(graph, _serializationOptions);
    }
}